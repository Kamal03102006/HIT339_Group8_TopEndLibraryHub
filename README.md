# Top End Library Hub

We are HIT339 Distributed Development Group 8. We expanded Kamalpreet's Assignment 2 library system into seven connected modules and access channels.

| Member | Student number | Main responsibility |
| --- | --- | --- |
| Kamalpreet | S389008 | Coordination, holds, notifications, shared circulation and integration |
| Harsimran Singh | S388232 | Branches, reception desks, transfers and installation guide |
| Mehakpreet Kaur | S390303 | Self-service kiosk, secure catalogue API and patron guide |
| Sandesh Bhattarai | S390535 | CSV importer, manager analytics and CSV/PDF exports |

## Current release and verification

The 16 October integration combines the lead's tested `9b28459` source with Sandesh's `da0f483` corrections and the kiosk/API/branch work in those histories. Shared checkout, reservation, transfer and notification rules are retained. General availability now also drives manager reports. The importer creates physical copies for Quantity, validates PublicationYear and explains skipped rows. CSV/PDF exports include fine and inventory audit details with AUD amounts and Darwin timestamps.

Our Windows build and all 33 tests passed with none skipped at b79faf3. All ten HTTPS API checks passed. Actual imports, matching manager CSV/PDF, a non-zero payment transition and selected route/logout checks are recorded in [release checks](docs/RELEASE_CHECKS_16OCT.md). The final source includes documentation and a duplicate-button cleanup without changing report calculations. Our VM installation remains blocked.

The complete [Group Portfolio PDF](docs/HIT339_Group8_Portfolio.pdf) is the submission document. The cumulative package records the final commit; push the updated integration branch before submitting its repository link.

## Download and install

Use the final integrated branch identified in the submitted portfolio. During release checking, use `integration/group8-release-16oct` after importing/pushing the supplied Git bundle. Once reviewed and merged, the same version should be available on master. The installation guide must record the final commit actually tested.

```powershell
git clone https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub.git
cd HIT339_Group8_TopEndLibraryHub
git switch integration/group8-release-16oct
dotnet restore .\TopEndLibraryHub.csproj
dotnet dev-certs https --trust
```

Windows needs Git, the .NET 10 SDK, SQL Server Express LocalDB and a current browser. Visual Studio is optional for running the CLI, and must support the installed .NET 10 SDK when used. See [the full installation guide](docs/INSTALLATION_GUIDE.md) for setup, database history and VM checks. Do not generate a new migration just to run the project.

The application uses `ConnectionStrings:DefaultConnection`. Retain an existing database only when its history matches the seven committed migrations listed in the guide. For a fresh demonstration install, select a new database name in user-secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\MSSQLLocalDB;Database=HIT339_Group8_Demo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
$apiKey = [Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
dotnet user-secrets set "CatalogueApi:ApiKey" $apiKey
dotnet user-secrets set "Kiosk:BranchCode" "DAR"
dotnet build .\TopEndLibraryHub.csproj
dotnet run --launch-profile https
```

Keep the API key private. It is required for `/api/catalogue` routes; the API fails closed with 503 when no key is configured. Startup applies committed migrations and seeds demonstration roles, branches, desks and library data. Repeat startup should preserve existing records. This is a local assessment demonstration with simulated messages, not an external email/SMS/payment service.

## Branch locations and demonstration accounts

All locations use the same code and database setup. Staff selects Working location and chooses its active branch and matching reception desk. Kiosk location is trusted server configuration, using DAR, PAL or CAS as `Kiosk:BranchCode`. The browser cannot choose another location by posting an arbitrary branch ID at checkout.

| Physical branch | Code | Reception desk |
| --- | --- | --- |
| Darwin Hub | DAR | DAR-D01 |
| Palmerston Hub | PAL | PAL-D01 |
| Casuarina Hub | CAS | CAS-D01 |

| Local demonstration role | Email | Password |
| --- | --- | --- |
| Admin | admin@topendlibrary.local | Admin#2026! |
| Reception | reception@topendlibrary.local | Reception#2026! |
| Manager | manager@topendlibrary.local | Manager#2026! |
| Development patron Sky Nguyen / MBR-8001 | patron@topendlibrary.local | Patron#2026! |

The patron account is seeded only in Development and receives no staff role. Do not reuse these demonstration credentials for a deployed service. Existing real or differently named borrower records are not automatically linked by matching email.

## Navigate the modules

| Module | Route | Access and purpose |
| --- | --- | --- |
| Public catalogue | `/Catalogue` | Public search, item-type/status filters, physical location and general availability |
| Self-service kiosk | `/Kiosk`, `/Kiosk/Scan`, `/Kiosk/Account`, `/Kiosk/Hold` | Public browsing; authenticated linked active membership for private borrowing/hold actions |
| Catalogue API | `/api/catalogue`, `/available`, `/categories`, `/status`, `/{code}` | API-key protected read-only catalogue, eligible copies, categories and system/branch status |
| Notifications | `/Notifications` | Admin/Reception/Manager; simulated Email/SMS log and filtered recheck |
| Branch transfers | `/StaffLocation`, `/Transfers` | Admin/Reception; choose location, dispatch and receive at destination |
| Holds and waitlist | `/Holds`, `/Holds/Create`; patron `/Kiosk/Hold` | FIFO requests, cancellation, repair/transfer readiness and private patron progress |
| Importer | `/CsvImport` | Admin; CSV upload with physical-copy creation, validation and row outcomes |
| Manager reports | `/Manager`, `/Manager/ExportCsv`, `/Manager/ExportPdf` | Manager/Admin; hold-aware availability, borrowing statistics and fine/inventory audits |
| Circulation | `/Loans`, `/Loans/Borrow`, `/Loans/Return` | Admin/Reception; shared checkout/return rules and recorded fine payments |

API item queries accept `limit` (1-100), `category` (Book/Music/Toy), and an active `branchId`; catalogue search also accepts `search` (at most 100 characters). For example, `/api/catalogue/available?category=Book&branchId=1&limit=20`. Status reports service/database health and configured active branch flags; live opening hours are not configured. API responses contain no patron account records.

A physical Available item may be reserved and unavailable for general borrowing. The rightful pickup member can collect it even while another member waits. The shared circulation service enforces active membership, physical location, no active transfer/loan and reservation ownership inside the same per-item transaction. A filtered unique index is the final duplicate-loan guard. Due dates use Darwin calendar days; timestamps are stored in UTC and displayed as ACST. Late charges use AUD 1.50 per overdue calendar day.

## Test the integrations

Stop the running app before rebuilding. From the repository root in PowerShell:

```powershell
.\scripts\Test-Release.ps1 -IncludeSqlServer
```

The unit/fixture checks exercise production fine, payment, reporting, import and API-key code. The SQL Server checks use newly generated `TopEndLibraryHub_Tests_*` databases and remove only those databases. They do not reset the application database. Without `-IncludeSqlServer` or `TOPEND_TEST_SQLSERVER`, three SQL checks are explicitly skipped. Their migration/model checks and InMemory fixture results are not a successful VM demonstration.

With the local application running and API configured:

```powershell
.\scripts\Test-CatalogueApi.ps1 -BaseUrl 'https://localhost:7197'
```

The script asks for the API key without displaying it and checks missing/wrong/valid keys, read-only methods, categories/status, validation and absence of private account fields. Use the actual HTTPS address printed by the application. Keep its output and the test TRX file. Finish the targeted browser and VM checks in [release checks](docs/RELEASE_CHECKS_16OCT.md).

Representative CSVs are in [docs/samples](docs/samples); [the import/report guide](docs/IMPORT_AND_REPORT_GUIDE.md) gives exact expected sample outcomes. Synthetic generated exports are labelled under [test evidence](docs/test-evidence). Historical screenshots and their limits are in [the screenshot index](docs/screenshots/README.md).

## Documentation and submission

- [Installation guide](docs/INSTALLATION_GUIDE.md) and [patron user guide](Documentation/PatronUserGuide.md)
- [Current requirements](docs/REQUIREMENTS.md), [group agreement/plan/task records](docs/GROUP_WORK_RECORD.md) and [minutes](docs/MEETING_MINUTES.md)
- [Integration and journeys](docs/ARCHITECTURE_AND_JOURNEYS.md), [assessment checklist](docs/ASSESSMENT_CHECKLIST.md) and [remaining work](docs/REMAINING_WORK.md)
- [Harsimran's individual account](docs/individual/HARSIMRAN_SINGH.md), [Mehakpreet's reflection](docs/individual/MEHAKPREET_KAUR.md), [Sandesh's account](docs/individual/SANDESH_BHATTARAI.md) and [Kamalpreet's report](docs/individual/KAMALPREET.md)
- [Submission guide](docs/SUBMISSION_GUIDE.md) and [AI acknowledgement](docs/AI_ACKNOWLEDGEMENT.md)

Submit one Group Portfolio PDF plus the final source repository link. The portfolio includes all four named individual sections, installation/patron guides, project-management artifacts, integration diagrams and user journeys. Final source revision, actual test outcomes and marker access must be checked before upload. The official deadline is 16 October 2026 at 23:59 Darwin unless an extension is confirmed in writing.
