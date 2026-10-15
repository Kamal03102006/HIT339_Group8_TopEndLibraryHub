# Top End Library Hub

HIT339 Distributed Development, Group 8. Kamalpreet's Assignment 2 .NET 10 MVC application is being expanded into a fictional family lending service for books, music and toys at Darwin, Palmerston and Casuarina hubs.

Group: Kamalpreet (S389008), Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai. Internal target: 15 October 2026. Submission: 16 October 2026, 23:59 Darwin/ACST.

## Revision and verification

Working checkpoint a1e8190 on feature/holds-notifications combines baseline dbbc540 with Mehakpreet's kiosk/API contribution from 3b925ee. The applied circulation integration includes shared checkout/returns, eligible FIFO allocation, protected transfers and simulated pickup messages. Kamalpreet's Windows build succeeded; the supplied Get-Migration output shows all seven migrations Applied=True.

Five committed screenshots establish duplicate rejection, waiting order and cancellation. Later supplied images show Noah Ready at Darwin and damaged-copy borrowing rejection. The 14 October Windows reports cover reserved-member checkout, repeat checkout, repair allocation, protected Darwin-to-Palmerston transfer, collection and next-member handover. A damaged return preserved a new waiting request; the subsequent repair increased pickup-only history from four to six records, and the filtered count stayed 6 → 6 after restart. See the test records for exact inputs, results and evidence limits.

Remaining kiosk checkout/API/security and simultaneous-checkout acceptance, fine/loan notification checks, clean-VM verification, importer/report handoff and the final portfolio are outstanding. The final submission branch/tag will identify the completed seven-module version.

The subsequent patron hold patch adds Identity-linked `/Kiosk/Hold`, shared staff/patron queue validation and signed-in public-kiosk Finish/idle handling. On 15 October Kamalpreet supplied a clean Windows build after pinning System.Security.Cryptography.Pkcs to 9.0.21; the include-transitive audit reported no vulnerable packages given its current sources. She reported own-request/FIFO/duplicate/private-account and Finish/Back outcomes. Two inspected photographs show inactive and unlinked membership access blocked. JavaScript-disabled expiry, forged-input/antiforgery checks and the remaining module acceptance are pending. The tested working source still needs its pushed revision identified. See [the patron hold test record](docs/PATRON_HOLD_TEST_RECORD.md) for exact evidence and limitations.

## Modules and routes

| Module | Route | Access | Status |
| --- | --- | --- | --- |
| Catalogue | /Catalogue | Public | Shared rule excludes loans, transit and reservations; final public/API runtime checks pending. |
| Kiosk | /Kiosk, /Kiosk/Scan, /Kiosk/Account | Public browsing; authenticated private actions | Private hold progress, idle and Finish/Back reported checked; inactive/unlinked access images inspected. Shared checkout, remaining security and browser coverage pending. |
| Catalogue API | /api/catalogue, /available, /categories, /status, /{code} | External X-API-Key over HTTPS | Read-only; key outside Git; health is system/database status. |
| Working location | /StaffLocation | Admin/Reception | Active physical branch/desk selection. |
| Holds | /Holds, /Holds/Create; /Kiosk/Hold | Staff desk: Admin/Reception; patron form: authenticated active linked member | Staff repair/transfer/collection journey has inspected and reported evidence. New patron creation, shared duplicates/FIFO and private progress reported checked; PH03 access images inspected. POST security checks pending. |
| Transfers | /Transfers, /Transfers/Create | Admin/Reception | Harsimran's dispatch/receipt with shared protected hold handover; main flow reported tested. |
| Lending | /Loans, /Loans/Borrow, /Loans/Return | Admin/Reception | Shared service; usable and NeedsRepair returns reported tested. Kiosk/concurrency checks pending. |
| Simulated messages | /Notifications | Admin/Reception/Manager | Automatic startup/minute checks for loans and ready holds; no real messages sent. |
| Manager | /Manager | Admin/Manager | Baseline statistics; expanded revenue/health exports pending handoff. |
| Import | Route not yet verified | Admin | Pending handoff and sample CSVs. |

## Install and run

Prerequisites: .NET 10 SDK, Git, Visual Studio with ASP.NET/web development support, and SQL Server LocalDB. Open TopEndLibraryHub.slnx, select the web project, restore, build and run with the HTTPS profile. The final clean-VM installation test remains outstanding.

```powershell
git clone --branch feature/holds-notifications https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub.git
cd HIT339_Group8_TopEndLibraryHub
dotnet restore
dotnet build
dotnet run --launch-profile https
```

Use the URL printed by the app. Startup applies missing EF migrations and repeat-safe fictional branch/stock/staff seeds. Review a migration failure; preserve existing loan/hold history. The new identity migration creates a unique active-loan index and will reject inconsistent duplicate active loans.

In Visual Studio's Package Manager Console, with TopEndLibraryHub selected as the default project, `Update-Database -Context ApplicationDbContext` applies the same pending migrations before launch. The circulation patch contains Mehak's identity migration; it does not require generating a new migration for these service/view changes.

## Fictional locations

| Code | Location | Reception desk |
| --- | --- | --- |
| DAR | Darwin Hub | DAR-D01 |
| PAL | Palmerston Hub | PAL-D01 |
| CAS | Casuarina Hub | CAS-D01 |

Staff select **Front desk → Working location** before lending, returns, stock creation or dispatch/receipt. This is a physical branch, separate from a source-code Git branch. A usable return is received at the selected physical branch; a transfer keeps its origin until destination receipt.

In Development only, the patch seeds a separate linked patron Sky Nguyen / MBR-8001 / patron@topendlibrary.local / Patron#2026! with no staff role. Existing memberships stay unlinked. Kiosk checkout's trusted branch defaults to DAR; configuration can select PAL/CAS for testing.

Configure an API key outside committed source:

```powershell
dotnet user-secrets set "CatalogueApi:ApiKey" "<your-long-random-key>"
dotnet user-secrets set "Kiosk:BranchCode" "DAR"
```

Send X-API-Key; the unconfigured API returns 503, missing/wrong key 401. Private kiosk screens and nonstaff sign-in use a two-minute inactivity/expiry policy; Finish signs out and clears the session. Test privacy and Back behaviour on the actual browser.

## Test the integrations

For a fresh installation, follow [the circulation demo guide](docs/CIRCULATION_DEMO_GUIDE.md). It supplies fictional borrower and toy fields, physical desks, workflow actions and pickup-only counts. It uses a new copy TY-9101 so the recorded TY-9001 history remains intact.

The [integration test record](docs/CIRCULATION_INTEGRATION_TEST_RECORD.md) preserves the actual 14 October run. Its existing queues have progressed: TY-0001 was collected by Noah, and TY-9001 has completed the two-family handover and a later repair cycle. Do not assume these records still have the starting states described in older test instructions.

When comparing pickup messages, set the item code, Message type Ready for pickup and Channel All channels, then click Filter records. The immediate Check loans and pickups now button currently returns to an unfiltered page; reapply the filters before comparing counts. Historical pickup messages remain visible after collection. Loan-confirmation messages count separately when All types is selected.

## Documentation

- [Requirements](docs/HIT339_Group8_Requirements_Draft.docx)
- [Assessment checklist](docs/ASSESSMENT_CHECKLIST.md): all 11 rubric categories, brief and final deliverables
- [Group work record](docs/GROUP_WORK_RECORD.md): tasks, decisions, governance and missing genuine member inputs
- [Hold evidence](docs/HOLD_WORKFLOW_TEST_RECORD.md) and [screenshot index](docs/screenshots/README.md)
- [Screenshot audit and coverage](docs/SCREENSHOT_AUDIT.md)
- [Whole-commit review and complete rubric progress](docs/PROJECT_PROGRESS_REVIEW.md)
- [Notification observations](docs/NOTIFICATION_TEST_RECORD.md)
- [UX issue, fix and retest](docs/UX_FEEDBACK_RECORD.md)
- [Integration diagram and journeys](docs/ARCHITECTURE_AND_JOURNEYS.md)
- [Patron guide](Documentation/PatronUserGuide.md) and [kiosk/API handoff](Documentation/Kiosk_Integration_Readme.md)

Written records are compiled from actual reports/source/screenshots with AI assistance. Genuine hours, team-contract acceptance, meetings, individual reflections/value contributions, final VM run and member handoffs remain outstanding. Working documents and diagrams are inputs to the required final portfolio PDF; the PDF is not complete.

## Assignment 2 baseline reference

The baseline details below describe the original application. They do not establish that the seven group expansions or their final integration pass.

### Baseline project information


- Unit: HIT339 Distributed Development
- Student: Kamalpreet
- Student number: S389008
- Framework: ASP.NET Core MVC with .NET 10
- Database: Microsoft SQL Server LocalDB
- Authentication: ASP.NET Core Identity
- Front-end: Razor views, Bootstrap, HTML and CSS

### Assignment 2 baseline features

### Public Catalogue

- Browse the complete collection without logging in.
- Search by title, library code, description, author, artist, genre, toy type or manufacturer.
- Filter items by type and availability status.
- Use the catalogue on desktop, tablet and mobile devices.

### Administrator

- Access a protected administration dashboard.
- Create, view, edit and delete books.
- Create, view, edit and delete music items.
- Create, view, edit and delete toys.
- Search and filter collection records.
- Prevent duplicate library codes.
- Prevent deletion of items that have associated loan records.

### Reception Staff

- Create, view, edit and delete borrower records.
- Search for borrowers by membership number, name, email or phone.
- Issue available items to active borrowers.
- Record returned items.
- Automatically calculate overdue fines.
- Record fine payments.
- View active, overdue and returned loans.
- View each borrower’s complete borrowing history.

### Manager

- View collection totals and availability statistics.
- Review item counts by type and status.
- View total, active, overdue and returned loans.
- Review fine totals, paid fines and outstanding fines.
- Identify frequently borrowed items.
- Review recent borrowing activity.

### Object-Oriented Design

The application uses an abstract Item superclass containing the properties shared by every library item:
- Library code
- Name
- Description
- Status
- Date added
The following classes inherit from Item:
- Book
- Music
- Toy
Each subclass contains information specific to that item type. Entity Framework Core uses table-per-hierarchy inheritance mapping to store the item types while preserving their object-oriented relationships.
The Borrower and Loan classes represent library members and borrowing transactions. Each loan is connected to one borrower and one library item.

### Roles and Demo Accounts

The application automatically creates the required roles and local demonstration accounts.

| Role | Email | Password |
| --- | --- | --- |
| Administrator | admin@topendlibrary.local | Admin#2026! |
| Reception | reception@topendlibrary.local | Reception#2026! |
| Manager | manager@topendlibrary.local | Manager#2026! |



These credentials are demonstration accounts intended only for local assessment and testing.

### Role Permissions

| Function | Public | Reception | Manager | Admin |
| --- | --- | --- | --- | --- |
| Browse public catalogue | Yes | Yes | Yes | Yes |
| Manage borrowers | No | Yes | No | Yes |
| Borrow and return items | No | Yes | No | Yes |
| Manage books, music and toys | No | No | No | Yes |
| View manager statistics | No | No | Yes | Yes |
| View administration dashboard | No | No | No | Yes |



Role-based authorization is enforced by the application controllers, not only by hiding navigation links.

### Business Rules

- Every physical library item must have a unique library code.
- Every borrower must have a unique membership number and email address.
- Membership numbers use the format MBR-0001.
- Only active borrowers can borrow items.
- Only items with an Available status can be borrowed.
- Borrowing an item changes its status to Borrowed.
- Returning an item changes its status back to Available.
- The normal loan period is 14 days.
- Overdue fines are calculated at $1.50 for each late day.
- A due date must be after the borrowed date.
- A returned date cannot be before the borrowed date.
- Database relationships restrict unsafe deletion of records with loan history.

### Validation and Security

The application includes:
- ASP.NET Core Identity authentication.
- Role-based authorization.
- Server-side and client-side model validation.
- Data annotation validation rules.
- Anti-forgery protection on submitted forms.
- Controlled model binding to reduce unwanted property updates.
- Unique database indexes for library codes, membership numbers and borrower emails.
- Restricted database deletion behaviour for loan relationships.
- Duplicate-record and database update error handling.

### Database Setup

The application uses Entity Framework Core with Microsoft SQL Server LocalDB.
When the application starts, it automatically:
1. Applies pending Entity Framework Core migrations.
2. Creates the Admin, Reception and Manager roles.
3. Creates the demonstration staff accounts.
4. Adds repeat-safe demonstration records when required.
Because the initialisation process is repeat-safe, restarting the application does not create duplicate accounts or demonstration data.

### Running the Application

### Visual Studio

1. Open the TopEndLibraryHub solution in Visual Studio.
2. Confirm that the TopEndLibraryHub project is selected as the startup project.
3. Build the solution using Build > Build Solution.
4. Run the application using the green HTTPS button.
5. Use the public catalogue without logging in or select Staff login to test a staff role.

### Command Line

From the project directory, run:
dotnet restore
dotnet build
dotnet run
Open the HTTPS address displayed in the terminal.

### Main Application Routes

| Page | Route |
| --- | --- |
| Home | / |
| Public catalogue | /Catalogue |
| Administration dashboard | /Admin |
| Books | /Books |
| Music | /Music |
| Toys | /Toys |
| Borrowers | /Borrowers |
| Loan records | /Loans |
| Borrow an item | /Loans/Borrow |
| Return an item | /Loans/Return |
| Manager dashboard | /Manager |



### Demonstration Data

The repeat-safe demonstration data includes:
- Book: BK-0001 — Discovering the Top End
- Music: MU-0001 — Sounds of the Wet Season
- Toy: TY-0001 — Build the Top End
- Borrower: MBR-0001 — Leah Johnson
- Active, overdue and returned loan examples

### Assignment 2 baseline testing record

The original Assignment 2 README recorded the following manual tests as completed successfully. These are historical baseline claims; they do not establish that the Group 8 expansions or integrations pass. Group notification results and remaining checks are recorded in [NOTIFICATION_TEST_RECORD.md](docs/NOTIFICATION_TEST_RECORD.md).
- Application builds and starts without errors.
- Database migrations and initial data creation complete successfully.
- Public users can search and filter the catalogue.
- Administrators can manage books, music and toys.
- Reception staff can manage borrowers and loan transactions.
- Borrowing changes an item’s status to Borrowed.
- Returning changes an item’s status to Available.
- On-time returns produce no fine.
- Overdue returns calculate the correct fine.
- Fine payments can be recorded.
- Borrowing history appears on borrower details.
- Managers can access collection, loan and fine statistics.
- Unauthorized roles receive an Access Denied response.
- Navigation options change according to the signed-in role.
- Pages adapt correctly to a narrow browser window.

### Project Structure

- Controllers — request handling, authorization and business workflows
- Data — Entity Framework database context and initialisation
- Models — domain entities and validation
- ViewModels — page-specific data models
- Views — Razor user interfaces
- Migrations — Entity Framework Core database migrations
- wwwroot — CSS, JavaScript and client-side libraries
- Program.cs — application services and middleware configuration
- DEVELOPMENT_LOG.md — planning, implementation and testing record
