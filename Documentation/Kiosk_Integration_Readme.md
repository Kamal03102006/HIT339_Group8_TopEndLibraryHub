# Kiosk integration handoff (11 October 2026)

## Implemented
- Optional IdentityUserId link, unique for non-null values, FK Restrict. Existing borrowers stay unlinked and retain loans.
- Authenticated account summary resolves borrower exclusively from UserManager.GetUserId(User).
- Active/inactive/unlinked membership handling; no account lookup by patron-supplied membership ID.
- Sign out and 5-minute non-sliding Identity authentication cookie. Catalogue remains public.
- Active loan exclusion in catalogue API; API-key middleware retained.
- SQL filtered unique index on active loans per item to prevent duplicates once checkout is integrated.

## CRITICAL BLOCKER
Checkout is intentionally **fail-closed**. `POST /Kiosk/Checkout` does NOT create a loan until the shared hold, transfer and branch restrictions are available and reviewed. The `Kiosk:SharedBorrowingRulesIntegrated` switch cannot bypass this safeguard. No success screenshot should be claimed for checkout yet.
The API available-items result checks status and active loans but not unfinished holds/transfers. Treat it as provisional, not authoritative borrowability.

## Migration
The checked-in migration `20261011090000_LinkPatronIdentity` adds nullable `Borrowers.IdentityUserId`, a filtered unique index, and a foreign key. It also changes the existing loan ItemId index to filtered unique for active loans. **Before applying**, verify there are no duplicate active loans for an item; if there are, resolve the inconsistent records with staff before running migration. `DbInitializer` applies migrations on startup.

## Link two fictional test accounts safely (authorised staff only)
Create two confirmed Identity accounts with distinct strong passwords using the existing Identity UI, and create two *new fictional* Borrower records using authorised staff workflows. Do not reuse existing borrower records based on matching email. In a controlled local demo, a staff member may link records in SQL after verifying both identities:
```sql
UPDATE Borrowers SET IdentityUserId = (SELECT Id FROM AspNetUsers WHERE NormalizedEmail = 'PATRON1@EXAMPLE.TEST') WHERE MembershipNumber = 'MBR-9001' AND IdentityUserId IS NULL;
UPDATE Borrowers SET IdentityUserId = (SELECT Id FROM AspNetUsers WHERE NormalizedEmail = 'PATRON2@EXAMPLE.TEST') WHERE MembershipNumber = 'MBR-9002' AND IdentityUserId IS NULL;
```
Check the `UPDATE` affected exactly one row each; use only staff-authorised demo data. Never run these updates against an existing real patron record without verification. Do not give patron accounts Admin, Reception, or Manager roles. Keep confirmed-account requirement.

## Testing evidence
1. Patron1 login -> Account shows only Patron1 loans/fines.
2. Patron2 login -> Account shows only Patron2 loans/fines.
3. Unlinked confirmed login -> explanatory message and no loan data.
4. Inactive linked borrower -> inactive message; checkout rejected.
5. Sign out -> protected Account and Scan redirect to login.
6. Wait >5 minutes -> login required again on next request; test with browser back.
7. Checkout -> safe pending-integration message, no new loan.
8. API key required; active-loan item absent from available results.

## Remaining team integration
Obtain final Hold, Transfer, Branch models and shared borrowing service from team leader/Harsimran. Checkout must enforce holds allocated to correct borrower and pickup branch, transfers, damage, loan limits and atomic transactions before enabling real borrowing. API must incorporate holds/transfers and branch opening hours. A kiosk timeout should also clear the on-screen display after inactivity; the current 5-minute server auth expiration protects private requests but does not auto-navigate an idle screen.

## Environment
Use `dotnet user-secrets set "CatalogueApi:ApiKey" "<long-random-secret>"` in the project directory (never commit the value). The project targets .NET 10. Run a full build and migration test in Visual Studio; this patch was generated without a local dotnet compiler.
