# Installation and running instructions

This guide covers the combined .NET 10 MVC release for all physical branches. It incorporates Harsimran's installation work and the later importer/report/kiosk configuration. The final commit and VM installation outcome must be recorded after actual verification. A successful VM installation is not yet claimed.

## Prerequisites

Use a supported Windows environment capable of running the .NET 10 SDK and SQL Server Express LocalDB, such as a suitable Windows 11 VM. Install Git, .NET 10 SDK, SQL Server Express LocalDB and a current browser. Visual Studio is optional; if used, it must support the installed SDK. The integration build was checked on Linux with SDK 10.0.402/runtime 10.0.13 and EF Core 10.0.10. LocalDB execution requires the Windows environment and is a separate check.

Verify `git --version`, `dotnet --info` and `sqllocaldb info`. Start the LocalDB instance if needed using `sqllocaldb start MSSQLLocalDB`. Check VM free disk/RAM before installing; retain VM/OS versions and the actual commands/results in the installation record.

## Obtain the final source

```powershell
git clone https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub.git
cd HIT339_Group8_TopEndLibraryHub
git switch integration/group8-release-16oct
git rev-parse HEAD
dotnet restore .\TopEndLibraryHub.csproj
dotnet dev-certs https --trust
```

The integration branch must first be pushed from the supplied bundle. After it is reviewed and merged, master can be used at the same final revision. Record the actual commit tested. The project is `TopEndLibraryHub.csproj`; the solution is `TopEndLibraryHub.slnx`.

## Select the database and configuration

For a fresh installation choose a new demonstration database through user-secrets; SQL Server creates it when startup migrations are applied:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\MSSQLLocalDB;Database=HIT339_Group8_Demo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
$apiKey = [Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
dotnet user-secrets set "CatalogueApi:ApiKey" $apiKey
dotnet user-secrets set "Kiosk:BranchCode" "DAR"
dotnet build .\TopEndLibraryHub.csproj
dotnet run --launch-profile https
```

Keep the API secret private and do not paste it into committed files or evidence captures. Use the HTTPS URL printed by the application (the recorded profile uses port 7197). Startup applies committed migrations and creates repeat-safe demonstration data, roles, staff accounts and three branch/desks. Development also creates the separate linked Sky patron account described in README.

An existing lead database is compatible only when it follows this canonical history:

| Order | Migration |
| --- | --- |
| 1 | 00000000000000_CreateIdentitySchema |
| 2 | 20260906021222_AddLibraryDomainModels |
| 3 | 20261011030009_AddHoldsAndNotifications |
| 4 | 20261011090000_LinkPatronIdentity |
| 5 | 20261011133133_AddBranchesAndReceptionDesks |
| 6 | 20261012101617_AddItemTransfers |
| 7 | 20261014074832_LinkHoldsToPickupBranches |

The earlier consolidated `20261014104445_IntegrateHoldsBranchesTransfers` belongs to a different historical branch and is not added to this chain. If that history is present, retain the old database and choose a separate fresh demonstration database for this release. Do not delete user data, rewrite migration history or generate another migration to hide a duplicate-table error. The new report/import/service changes require no new schema migration.

In Visual Studio, `Get-Migration -Context ApplicationDbContext` shows the history; `Update-Database -Context ApplicationDbContext` can apply pending canonical migrations before launch. Startup does the same application step automatically. The offline model/script test checks for pending model differences and repeated creation of shared tables; it does not execute SQL Server migrations.

## Run at each physical location

Sign in with the local demonstration Admin or Reception account. Open Working location, select the branch and a matching active desk, and confirm the location displayed in the navigation. DAR/DAR-D01, PAL/PAL-D01 and CAS/CAS-D01 use identical software/setup. New inventory uses the staff's validated working branch. Ordinary edits preserve existing location; transfers move an item through dispatch and destination receipt.

A physical kiosk uses the trusted server configuration `Kiosk:BranchCode`; change it to PAL or CAS for a station at those branches, then restart. A patron cannot select a different checkout branch through form input. Use separate patron accounts and profiles for private-account testing. A new registration is not automatically linked to an existing borrower by email; use the controlled fictional linking process in Documentation/Kiosk_Integration_Readme.md.

## Verify installation and restart

On the VM, follow the guide from source download through migration/seed and launch. Record OS/VM, Git/.NET/LocalDB versions, final commit, configuration method, actual result and corrections. Verify login, location selection at each branch, catalogue, kiosk account, API with a local key, a safe fictional circulation/transfer journey, importer and manager exports. Stop/restart the app; check that previous demonstration records are retained and branches/desks are not duplicated. Keep screenshots or logs of meaningful outcomes. Run the supplied Windows tests and record their actual results separately.

## Known VM blocker and troubleshooting

Harsimran installed Git and the .NET SDK in the Darwin VM, but SQL Server 2022 LocalDB installation failed with Error 1920: SQL Server VSS Writer could not start. An administrator attempt also failed, and Event Viewer reported a startup timeout. Another VM/environment blocked installation through administrator restrictions. The underlying cause is not confirmed. These are failed attempts, not installation passes.

Use a suitable environment or obtain lecturer agreement to an alternative verification method. Repeating the same blocked installer does not establish that the instructions work. Build-file locks in a OneDrive project previously cleared after stopping the app/Visual Studio and shutting down build servers; generated bin/obj files can then be rebuilt. Do not remove source, screenshots, migrations or database files during cleanup.
