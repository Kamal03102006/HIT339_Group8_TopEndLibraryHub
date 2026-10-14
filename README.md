# Top End Library Hub

HIT339 Distributed Development, Group 8. The Assignment 2 .NET 10 MVC application is being expanded into a fictional family lending service for books, music and toys at Darwin Hub, Palmerston Hub and Casuarina Hub.

Group members: Kamalpreet, Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai. Internal completion target: 15 October 2026. Official submission: 16 October 2026, 23:59 Darwin/ACST.

## Current revision and implementation status

The latest verified group integration commit is `74668e1` on `feature/holds-notifications`. A subsequent local batch adds the staff holds desk, green interface and generated pickup-branch migration. Kamalpreet reports completing its three guided hold checks and applying the membership capitalisation fix. The newer pushed revision and individual screenshot results have not yet been confirmed.

Harsimran's branch, reception desk, transfer and item-location work is integrated. Mehakpreet's kiosk/API source is on `feature/mehakpreet-kiosk-api` at `3b925ee`; it is not yet merged into this working branch. CSV import and expanded report exports await verified handoff. Automatic hold allocation and shared checkout/transfer/notification protection remain in progress.

## Group module navigation

| Module | Route | Role | Status |
| --- | --- | --- | --- |
| Collection search | `/Catalogue` | Public | Baseline catalogue; green home search links to it. |
| Working location | `/StaffLocation` | Admin or Reception | Branch and reception-desk selection is integrated. |
| Staff holds desk | `/Holds` and `/Holds/Create` | Admin or Reception | Included in the local desk batch; three tests reported completed. |
| Transfers | `/Transfers` and `/Transfers/Create` | Admin or Reception | Dispatch and destination receipt integrated; hold handover pending. |
| Loan records and lending | `/Loans`, `/Loans/Borrow`, `/Loans/Return` | Admin or Reception | Baseline lending with location/transit guards; reservation integration pending. |
| Simulated notifications | `/Notifications` | Admin, Reception or Manager | Loan worker and logs implemented; pickup workflow integration pending. |
| Manager statistics | `/Manager` | Admin or Manager | Baseline dashboard; group exports await verified delivery. |
| Kiosk and read-only API | Mehakpreet's separate branch | Role/credential rules in that branch | Identity-linked account source exists; checkout remains blocked pending shared circulation. |
| CSV import | Final route pending | Admin | Verified implementation and sample input files pending. |

Do not treat a route from an unmerged member branch as present in the integrated application.

## Download and run the working source

Use the .NET 10 SDK, Git, Visual Studio with ASP.NET and web development support, and SQL Server LocalDB. Open `TopEndLibraryHub.slnx`, choose TopEndLibraryHub as the startup project, restore packages, build and run with the HTTPS profile. The connection string is configured in the project's development settings. The startup initializer applies pending migrations and creates repeat-safe demonstration data. A clean VM run of the final installation guide is still required by the brief.

For a separate checkout of the current working branch:

```powershell
git clone --branch feature/holds-notifications https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub.git
cd HIT339_Group8_TopEndLibraryHub
dotnet restore
dotnet build
dotnet run --launch-profile https
```

Open the URL printed by the application. Sign in with a local demonstration staff account when testing a protected workflow. Existing baseline account information is retained below.

## Physical branch and reception desk setup

Startup seeding creates the following fictional locations and desks without adding duplicates on each restart:

| Branch code | Location | Desk code |
| --- | --- | --- |
| DAR | Darwin Hub | DAR-D01 |
| PAL | Palmerston Hub | PAL-D01 |
| CAS | Casuarina Hub | CAS-D01 |

As Admin or Reception, use **Front desk > Working location** to select an active branch and desk. A source-code Git branch chooses a revision; this working-location selection chooses a physical operating branch. New stock is assigned to the selected branch. A dispatch keeps the origin location until the destination receives it; items in transit cannot be checked out.

## Holds desk integration checks

Register two active fictional borrowers who do not already have the selected item on loan. Guided examples are `MBR-9001` (Asha Patel) and `MBR-9002` (Noah Wilson). Use an actual Borrowed or Damaged item with no active holds, such as `TY-0001` if its current state meets those conditions.

1. Place the first member's hold and try the same member/item request again. The duplicate should be rejected.
2. Add the second member's request. Their eligible waiting positions should be 1 then 2, even when newest requests display first.
3. Cancel the first waiting request. Keep its cancelled history and move the second eligible request to position 1.

Kamalpreet reports completing these procedures. Their screenshots are still on the laptop; detailed outcomes will be matched to the images when available. This batch does not yet demonstrate automatic allocation after return or repair.

Membership validation now accepts uppercase, lowercase and mixed-case `MBR` prefixes; existing controller normalization stores uppercase. Kamalpreet reported applying this fix after discovering that visual capitalization had concealed invalid lowercase input. The specific lowercase registration retest is tracked separately.

## Documentation and evidence

- [Requirements](docs/HIT339_Group8_Requirements_Draft.docx): seven modules, acceptance checks and updated design decisions.
- [Assessment checklist](docs/ASSESSMENT_CHECKLIST.md): all eleven rubric categories and submission deliverables.
- [Group work record](docs/GROUP_WORK_RECORD.md): allocations, decisions, progress, governance and member inputs still needed.
- [Hold test record](docs/HOLD_WORKFLOW_TEST_RECORD.md): guided inputs, reported completion and remaining checks.
- [Notification test record](docs/NOTIFICATION_TEST_RECORD.md): earlier observed results and final checks still required.
- [UX feedback record](docs/UX_FEEDBACK_RECORD.md): observed capitalisation problem, change and retest status.
- [Screenshot evidence](docs/screenshots/README.md): where to copy the three images using their existing names.

Written records are compiled from the lead's observations and member reports with AI assistance. Real outcomes, hours, meetings and individual contributions remain subject to group review. The final portfolio PDF, VM verification, final patron guide, diagrams and user journey maps are still being assembled.

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
