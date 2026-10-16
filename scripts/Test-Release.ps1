param([switch]$IncludeSqlServer)
$ErrorActionPreference = 'Stop'
Push-Location (Split-Path $PSScriptRoot -Parent)
$previousConnection = $env:TOPEND_TEST_SQLSERVER
try {
    & dotnet build .\TopEndLibraryHub.csproj --disable-build-servers
    if ($LASTEXITCODE -ne 0) { throw 'Build failed. Stop here and retain the output.' }
    if ($IncludeSqlServer -and [string]::IsNullOrWhiteSpace($env:TOPEND_TEST_SQLSERVER)) {
        $env:TOPEND_TEST_SQLSERVER = 'Server=(localdb)\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true'
    }
    & dotnet test .\TopEndLibraryHub.Tests\TopEndLibraryHub.Tests.csproj --logger 'trx;LogFileName=group8-windows.trx'
    if ($LASTEXITCODE -ne 0) { throw 'Tests failed. Retain the results; do not mark them passed.' }
    if (-not $IncludeSqlServer) { Write-Host 'SQL Server tests were skipped unless TOPEND_TEST_SQLSERVER was already configured.' }
    Write-Host 'Results: TopEndLibraryHub.Tests\TestResults\group8-windows.trx'
}
finally {
    $env:TOPEND_TEST_SQLSERVER = $previousConnection
    Pop-Location
}
