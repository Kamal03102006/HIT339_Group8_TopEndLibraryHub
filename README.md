# Top End Library Hub

Top End Library Hub is a responsive ASP.NET Core MVC application for managing a library collection of books, music and toys. It provides dedicated functions for administrators, reception staff, managers and members of the public.

## Project Information

- Unit: HIT339 Distributed Development
- Student: Kamalpreet
- Student number: S389008
- Framework: ASP.NET Core MVC with .NET 10
- Database: Microsoft SQL Server LocalDB
- Authentication: ASP.NET Core Identity
- Front-end: Razor views, Bootstrap, HTML and CSS

## Main Features

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

## Object-Oriented Design

The application uses an abstract `Item` superclass containing the properties shared by every library item:

- Library code
- Name
- Description
- Status
- Date added

The following classes inherit from `Item`:

- `Book`
- `Music`
- `Toy`

Each subclass contains information specific to that item type. Entity Framework Core uses table-per-hierarchy inheritance mapping to store the item types while preserving their object-oriented relationships.

The `Borrower` and `Loan` classes represent library members and borrowing transactions. Each loan is connected to one borrower and one library item.

## Roles and Demo Accounts

The application automatically creates the required roles and local demonstration accounts.

| Role | Email | Password |
|---|---|---|
| Administrator | admin@topendlibrary.local | Admin#2026! |
| Reception | reception@topendlibrary.local | Reception#2026! |
| Manager | manager@topendlibrary.local | Manager#2026! |

These credentials are demonstration accounts intended only for local assessment and testing.

## Role Permissions

| Function | Public | Reception | Manager | Admin |
|---|---:|---:|---:|---:|
| Browse public catalogue | Yes | Yes | Yes | Yes |
| Manage borrowers | No | Yes | No | Yes |
| Borrow and return items | No | Yes | No | Yes |
| Manage books, music and toys | No | No | No | Yes |
| View manager statistics | No | No | Yes | Yes |
| View administration dashboard | No | No | No | Yes |

Role-based authorization is enforced by the application controllers, not only by hiding navigation links.

## Business Rules

- Every physical library item must have a unique library code.
- Every borrower must have a unique membership number and email address.
- Membership numbers use the format `MBR-0001`.
- Only active borrowers can borrow items.
- Only items with an `Available` status can be borrowed.
- Borrowing an item changes its status to `Borrowed`.
- Returning an item changes its status back to `Available`.
- The normal loan period is 14 days.
- Overdue fines are calculated at `$1.50` for each late day.
- A due date must be after the borrowed date.
- A returned date cannot be before the borrowed date.
- Database relationships restrict unsafe deletion of records with loan history.

## Validation and Security

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

## Database Setup

The application uses Entity Framework Core with Microsoft SQL Server LocalDB.

When the application starts, it automatically:

1. Applies pending Entity Framework Core migrations.
2. Creates the Admin, Reception and Manager roles.
3. Creates the demonstration staff accounts.
4. Adds repeat-safe demonstration records when required.

Because the initialisation process is repeat-safe, restarting the application does not create duplicate accounts or demonstration data.

## Running the Application

### Visual Studio

1. Open the `TopEndLibraryHub` solution in Visual Studio.
2. Confirm that the `TopEndLibraryHub` project is selected as the startup project.
3. Build the solution using **Build > Build Solution**.
4. Run the application using the green HTTPS button.
5. Use the public catalogue without logging in or select **Staff login** to test a staff role.

### Command Line

From the project directory, run:

```bash
dotnet restore
dotnet build
dotnet run
```

Open the HTTPS address displayed in the terminal.

## Main Application Routes

| Page | Route |
|---|---|
| Home | `/` |
| Public catalogue | `/Catalogue` |
| Administration dashboard | `/Admin` |
| Books | `/Books` |
| Music | `/Music` |
| Toys | `/Toys` |
| Borrowers | `/Borrowers` |
| Loan records | `/Loans` |
| Borrow an item | `/Loans/Borrow` |
| Return an item | `/Loans/Return` |
| Manager dashboard | `/Manager` |

## Demonstration Data

The repeat-safe demonstration data includes:

- Book: `BK-0001` — Discovering the Top End
- Music: `MU-0001` — Sounds of the Wet Season
- Toy: `TY-0001` — Build the Top End
- Borrower: `MBR-0001` — Leah Johnson
- Active, overdue and returned loan examples

## Testing Completed

The following manual tests were completed successfully:

- Application builds and starts without errors.
- Database migrations and initial data creation complete successfully.
- Public users can search and filter the catalogue.
- Administrators can manage books, music and toys.
- Reception staff can manage borrowers and loan transactions.
- Borrowing changes an item’s status to `Borrowed`.
- Returning changes an item’s status to `Available`.
- On-time returns produce no fine.
- Overdue returns calculate the correct fine.
- Fine payments can be recorded.
- Borrowing history appears on borrower details.
- Managers can access collection, loan and fine statistics.
- Unauthorized roles receive an Access Denied response.
- Navigation options change according to the signed-in role.
- Pages adapt correctly to a narrow browser window.

## Project Structure

- `Controllers` — request handling, authorization and business workflows
- `Data` — Entity Framework database context and initialisation
- `Models` — domain entities and validation
- `ViewModels` — page-specific data models
- `Views` — Razor user interfaces
- `Migrations` — Entity Framework Core database migrations
- `wwwroot` — CSS, JavaScript and client-side libraries
- `Program.cs` — application services and middleware configuration
- `DEVELOPMENT_LOG.md` — planning, implementation and testing record