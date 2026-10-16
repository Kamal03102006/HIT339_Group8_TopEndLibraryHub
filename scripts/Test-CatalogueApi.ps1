param([string]$BaseUrl = 'https://localhost:7197', [Security.SecureString]$ApiKey)
$ErrorActionPreference = 'Stop'
$uri = [Uri]$BaseUrl
if (-not $uri.IsLoopback) { throw 'Use the local running application URL for this test.' }
if ($null -eq $ApiKey) { $ApiKey = Read-Host 'Catalogue API key (hidden)' -AsSecureString }
$pointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($ApiKey)
try { $plainKey = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer) }
finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer) }
function Request([string]$Path, [string]$Key, [string]$Method = 'GET') {
    $request = [Net.HttpWebRequest]::Create($BaseUrl.TrimEnd('/') + $Path)
    $request.Method = $Method
    $request.Timeout = 15000
    if (-not [string]::IsNullOrEmpty($Key)) { $request.Headers.Add('X-API-Key', $Key) }
    try { $response = $request.GetResponse() }
    catch [Net.WebException] {
        $response = $_.Exception.Response
        if ($null -eq $response) { throw }
    }
    try {
        $reader = New-Object IO.StreamReader($response.GetResponseStream())
        try { $body = $reader.ReadToEnd() } finally { $reader.Dispose() }
        return [PSCustomObject]@{ Status = [int]$response.StatusCode; Body = $body }
    } finally { $response.Dispose() }
}
$results = New-Object 'System.Collections.Generic.List[object]'
try {
    $cases = @(
        @{Name='Missing API key'; Path='/api/catalogue'; Key=''; Expected=401},
        @{Name='Wrong API key'; Path='/api/catalogue'; Key=([Guid]::NewGuid().ToString()); Expected=401},
        @{Name='Catalogue'; Path='/api/catalogue?limit=2'; Key=$plainKey; Expected=200},
        @{Name='Available copies'; Path='/api/catalogue/available'; Key=$plainKey; Expected=200},
        @{Name='Categories'; Path='/api/catalogue/categories'; Key=$plainKey; Expected=200},
        @{Name='Operating status'; Path='/api/catalogue/status'; Key=$plainKey; Expected=200},
        @{Name='Invalid limit'; Path='/api/catalogue?limit=0'; Key=$plainKey; Expected=400},
        @{Name='Invalid category'; Path='/api/catalogue/available?category=Unknown'; Key=$plainKey; Expected=400},
        @{Name='Missing item'; Path='/api/catalogue/ZZ-9999'; Key=$plainKey; Expected=404},
        @{Name='Read-only POST'; Path='/api/catalogue'; Key=$plainKey; Expected=405; Method='POST'}
    )
    foreach ($case in $cases) {
        $method = if ($case.ContainsKey('Method')) { $case.Method } else { 'GET' }
        $response = Request $case.Path $case.Key $method
        $passed = $response.Status -eq $case.Expected
        if ($response.Status -eq 200 -and $response.Body -match '"(Email|Phone|MembershipNumber|IdentityUserId|BorrowerId)"\s*:') { $passed = $false }
        $results.Add([PSCustomObject]@{ Case=$case.Name; Expected=$case.Expected; Actual=$response.Status; Passed=$passed })
    }
    $results | Format-Table -AutoSize
    if (@($results | Where-Object { -not $_.Passed }).Count -gt 0) { throw 'At least one API check failed. Retain this output.' }
    Write-Host 'API checks passed. The API key was not printed.'
} finally { $plainKey = $null }
