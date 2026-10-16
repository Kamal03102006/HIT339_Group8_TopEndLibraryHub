# Release verification on 16 October 2026

This record covers the prepared combined source. It separates automated results from the Windows, browser and VM outcomes still to be supplied. Earlier screenshots remain historical evidence; their names and captions do not imply that a new version was tested.

## Automated checks completed

The application and Razor views compile using .NET SDK 10.0.402, runtime 10.0.13 and EF Core 10.0.10 on Ubuntu 24.04. The final test run and package audit are recorded in [test evidence](test-evidence/README.md).

The checks call production code for fine calculations and payments, report availability/totals, CSV validation and physical-copy quantities, CSV escaping, PDF generation, API-key middleware, API filters, role/antiforgery metadata and notification-filter retention. The fixed clock crosses Darwin midnight so date calculations do not depend on the build machine's timezone. The payment test moves a non-zero recorded fine from unpaid to paid, preserves the total and verifies that a repeated payment does not replace its timestamp.

Database fixtures use EF InMemory. They verify application query rules, not SQL Server translation, foreign keys, filtered indexes, transactions or locking. A separate SQL Server model/script check finds no pending model changes, retains the seven canonical migrations and checks that shared tables are created once. This script check does not execute the migrations.

The generated CSV/PDF use a labelled synthetic fixture with 13 items, 3 generally available copies, 3 reserved copies, 2 in transit, AUD 9.00 recorded fines (3.00 paid / 6.00 unpaid) and a separate AUD 1.50 current-loan estimate. These are test data, not screenshots of the members' databases. The two-page PDF was visually checked for readable columns, dates, amounts and page breaks.

## Windows SQL Server checks still required

Stop the application and run this once in PowerShell from the repository root:

```powershell
.\scripts\Test-Release.ps1 -IncludeSqlServer
```

The three SQL tests apply the real migration chain to a new, isolated `TopEndLibraryHub_Tests_*` database and exercise: rightful ready-hold collection while another member waits; simultaneous checkout producing only one active loan; and a late return recording an unpaid fine while promoting the next hold. Only the isolated test database is removed. Keep the TRX output and report failures accurately. These three tests are skipped when no SQL Server connection is configured; a skip is not a pass.

## Targeted application checks

| Check | Exact action | Expected result | Current evidence |
| --- | --- | --- | --- |
| CSV quantities and duplicates | Admin imports books-valid.csv into an active branch, then repeats it | First run: 2 accepted rows / 3 copies; repeat: 0 copies, existing ISBNs explained | Production importer fixture passes; final Windows upload pending |
| Invalid CSV rows | Import books-mixed-validation.csv and books-missing-header.csv | Valid row accepted; Quantity=0 / Year=0 rejected with reasons; missing header rejects upload | Production fixture passes; final browser outcome pending |
| Report reconciliation | Manager opens dashboard and downloads CSV/PDF from the same stable dataset | Same availability/fine totals; inventory/fine audit rows explain them; AUD and ACST labels | Synthetic exporter/query tests and PDF inspection; final Windows files pending |
| Non-zero fine payment | Return a fictional overdue loan, note fine, pay it, export again | Unpaid decreases and paid increases by the same amount; total unchanged; payment date present | Production payment test passes; real SQL late-return test pending |
| API | Start app with a private API key; run scripts/Test-CatalogueApi.ps1 | Missing/wrong key 401; valid GET 200; invalid filters 400; POST 405; no patron fields | Middleware/query tests pass; actual HTTPS requests pending |
| Privileged routes | Signed-out and Patron accounts request importer and manager exports | Login challenge / access denied; no export data | Authorization metadata checked; browser challenge pending |
| Kiosk account privacy | Use linked active, inactive and unlinked fictional accounts; try another member's ID and omit antiforgery token | Ownership remains signed-in member; inactive/unlinked borrowing blocked; invalid POST rejected | Earlier account captures plus current source/metadata tests; forged POST check pending |
| Kiosk idle expiry | Disable JavaScript, leave patron session idle over two minutes, then request private data | Session expires; no private account data returned | Earlier normal idle behaviour reported; server-only check pending |
| Notification filters | Filter pickup records, run Check active loans now again | Same filters remain selected; duplicate events do not multiply | New redirect regression passes; earlier event counts reported |

Use fictional memberships/items, record the actual commit and avoid deleting existing data. The existing FIFO, cancellation, repair, transfer and pickup photographs do not need to be repeated merely to rename their files.

## Installation and submission limits

Harsimran's VM attempt failed during LocalDB installation. No successful VM run or lecturer-approved alternative is recorded. The final one-PDF portfolio has not been assembled by this code update. Mehak's 48-hour revision is a proposal pending her confirmation; Sandesh's later hours and Kamalpreet's later active time are unrecorded. The extension request is not an approval. These items remain visible in [remaining work](REMAINING_WORK.md).
