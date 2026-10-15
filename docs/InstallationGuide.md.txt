# TopEndLibraryHub Installation and Running Guide

Author: Harsimran Singh
Group: HIT339 Group 8
Branch covered: harsimran-branches

This guide explains how to download, configure and run the
multi-branch library application. All library locations use the same
code and select their working branch and reception desk after login.

## 1 Prerequisites

Install the following on the computer or Windows VM where the
application will run:

- Git for Windows.
- .NET 10 SDK.
- SQL Server Express LocalDB.
- A web browser.

Visual Studio with ASP.NET and web development tools is optional
when using the command-line instructions below.

Development computer versions recorded:
- .NET SDK: 10.0.400.
- ASP.NET Core runtime: 10.0.11.
- Entity Framework Core packages: 10.0.10.
- Target framework: net10.0.

The Windows VM successfully installed .NET SDK 10.0.402 and Git
2.56.0.windows.2. LocalDB installation remains blocked.

Check prerequisites in a new terminal:

```powershell
dotnet --info
git --version
sqllocaldb versions
sqllocaldb info
```

An empty instance list does not necessarily mean LocalDB is missing.
A "command not recognized" message means installation or PATH needs
checking.

## 2 Download the Repository

Run these commands in PowerShell from a folder where you want
a new checkout:

```powershell
git clone --branch harsimran-branches https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub.git
cd HIT339_Group8_TopEndLibraryHub
git status
git rev-parse HEAD
```

Record the full commit returned by the last command in your test log.

These instructions cover the feature branch. Before final submission,
confirm the team's integrated branch and update this guide accordingly.

## 3 Locate the Application Project

From the repository folder, run:

```powershell
Get-ChildItem -Recurse -Filter TopEndLibraryHub.csproj
```

Change into the directory containing TopEndLibraryHub.csproj.
The following commands must run from that directory.

For Visual Studio, open the repository's solution file, or open
TopEndLibraryHub.csproj directly.

## 4 Database Configuration

Confirm appsettings.json contains:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HIT339_Group8_TopEndLibraryHub;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

This configuration uses Windows authentication. No database password
is needed.

appsettings.Development.json currently contains logging settings only.
Environment variables and user secrets can override configuration;
check them if the application connects to an unexpected database.

Use the same Windows account to run the application and manage its
LocalDB instance.

If MSSQLLocalDB does not exist, create it:

```powershell
sqllocaldb create MSSQLLocalDB
```

Start it:

```powershell
sqllocaldb start MSSQLLocalDB
```

Do not delete an existing database or LocalDB instance to troubleshoot
without first preserving its data.

## 5 Restore Build and Start

From the directory containing TopEndLibraryHub.csproj:

```powershell
dotnet restore
dotnet build
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

Approve the development certificate prompt.

Open the HTTPS address printed in the terminal. On the development
computer this was https://localhost:7197, but use the address printed
for your installation.

Keep the terminal open while using the application.
Press Ctrl+C to stop it.

Alternatively, build the solution in Visual Studio and start the
https launch profile.

## 6 Migrations and Seed Data

Program.cs calls DbInitializer.InitialiseAsync at startup.

The initializer:
1. Applies pending migrations using MigrateAsync.
2. Creates demonstration staff roles and accounts.
3. Seeds branches and reception desks.
4. Seeds demonstration items, borrower and loan records.

Do not generate new migrations merely to install the application.
Use the migrations committed with the selected version.

Branch-related migrations:
- AddBranchesAndReceptionDesks
- AddItemTransfers

The branch migration assigns existing single-location stock to
Darwin Hub before enforcing the item branch relationship.

Seeded locations:

| Branch code | Branch name | Desk code |
|---|---|---|
| DAR | Darwin Hub | DAR-D01 |
| PAL | Palmerston Hub | PAL-D01 |
| CAS | Casuarina Hub | CAS-D01 |

Seeding checks existing branch and desk codes before inserting records.
Restart the application and verify that no duplicate records appear.

## 7 Demonstration Accounts

These accounts are for local assignment demonstrations only.

| Role | Email | Password |
|---|---|---|
| Admin | admin@topendlibrary.local | Admin#2026! |
| Reception | reception@topendlibrary.local | Reception#2026! |
| Manager | manager@topendlibrary.local | Manager#2026! |

Do not use these public demonstration credentials for a real deployment.

## 8 Select a Working Location

1. Sign in as Reception or Admin.
2. Open Front desk > Working location.
3. Select a branch and a matching reception desk.
4. Click Save working location.
5. Confirm the selected location appears below the navigation.

A desk from a different branch is rejected.
Only active branches and desks can be selected.

The selection is stored in the staff session and checked against
the signed-in user. Select the location again if the session expires.

All branches follow these same installation steps.
Separate code copies are not required for different locations.

## 9 Create and Edit Inventory

As Admin:
1. Select the working location.
2. Open Administration > Manage books, music or toys.
3. Create an item with a unique library code.
4. Confirm it belongs to the selected branch.

New items receive their branch from the validated working location.
Ordinary edits preserve the stored branch even when the staff
member is working from another branch.

Use transfers to change an item's location.

## 10 Transfer an Unreserved Item

1. Select the item's current branch and a matching desk.
2. Open Front desk > Interbranch transfers.
3. Click Create and dispatch transfer.
4. Select an eligible available item.
5. Choose a different active destination branch.
6. Dispatch it.

The item becomes InTransit, value 5. Its BranchId remains the origin
until receipt. Checkout is blocked during transit.

To receive:
1. Change the working location to the destination branch and desk.
2. Open the transfer list.
3. Click Receive at destination.

Successful receipt records the receiving staff member, desk and
timestamp, completes the transfer, and changes the item's BranchId.

The current standalone receipt action sets the item to Available.
Do not use this version for allocated holds until the team's shared
reservation rules are integrated.

Wrong-branch receipt is rejected. Repeated receipt must make no
additional changes.

## 11 Module Routes

| Route | Purpose |
|---|---|
| /Catalogue | Search the catalogue |
| /StaffLocation | Select branch and desk |
| /Transfers | Review transfers |
| /Transfers/Create | Dispatch an eligible item |
| /Loans/Borrow | Reception checkout |
| /Books | Manage books as Admin |
| /Music | Manage music as Admin |
| /Toys | Manage toys as Admin |

Transfer receipt uses an authenticated, anti-forgery-protected POST
from the transfer list, not a browser GET request.

## 12 Troubleshooting Recorded During Setup

### dotnet command not recognized

The VM did not initially have the SDK.
Installing the .NET 10 SDK and opening a new terminal resolved this.

### git command not recognized

The VM did not initially have Git.
Installing Git for Windows and opening a new terminal resolved this.

### LocalDB installation failed

On Darwin_harsimran, SQL Server 2022 LocalDB setup failed with
Error 1920 when SQL Server VSS Writer could not start.

Event Viewer recorded a service startup timeout.
An administrator installation attempt also failed.
The underlying cause has not been confirmed.

After rollback, SQLWriter was not registered and LocalDB was unavailable.
Changing the connection string does not resolve a missing database engine.

### Installer blocked

An installation attempt on Canberra showed
"This app has been blocked by your system administrator."

Use an administrator-approved installation environment.
Do not disable system security policies to bypass the restriction.

## 13 VM Verification Status

VM installation testing was attempted but has not passed.

Recorded environment:
- Oracle VirtualBox.
- Windows 10 Enterprise Evaluation version 1703.
- Observed OS build: 15063.13.

Completed:
- .NET SDK installation and command verification on Darwin.
- Git installation and command verification on Darwin.
- Capture of LocalDB installation failures and diagnostic evidence.

Still required on a suitable Windows VM:
- Successful LocalDB installation and startup.
- Repository download with commit recorded.
- Restore, build and application startup.
- Database migrations and seed verification.
- Branch and desk selection.
- Successful interbranch dispatch and receipt.
- Restart check for duplicate seed records.

Record each test's date, environment, commit, steps, expected result,
actual result and screenshot reference.

## 14 Final Integration Checks

Before treating this as the complete application installation guide:

- Confirm the integrated repository branch and commit.
- Review combined migration snapshots with the team.
- Add the final kiosk, API, importer, reporting and notification
  configuration requirements supplied by their owners.
- Connect held-item transfers to allocation and pickup notifications.
- Complete simultaneous checkout and dispatch protection and testing.
- Repeat installation testing against the final integrated version.