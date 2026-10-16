# Importing books and checking manager reports

## Administrative CSV import

Sign in as Admin and open Administration > Import books from CSV (`/CsvImport`). Select an active physical branch, choose a CSV file and select Import copies. The importer accepts Title, Author, ISBN, Category, Quantity and PublicationYear; Publisher is optional. Headings are case-insensitive. Each accepted row creates the requested number of physical Book copies, each with a unique BK code and the selected branch.

Quantity is a whole number from 1 to 100. PublicationYear must be from 1000 to the earlier of 2100 and next year. Other text fields follow the Book model's length limits. An ISBN already in the catalogue or accepted earlier in the file is skipped. Copies generated from one accepted row intentionally share an ISBN. A file must be non-empty CSV of at most 5 MB. Import is restricted to Admin and uses antiforgery validation.

The page reports imported rows, physical copies and skipped rows. Its row table explains invalid quantity/year, missing text, overlong fields and duplicate ISBNs. It displays up to 200 individual outcomes, while counts cover the full file. Malformed CSV/database failures do not commit a partial upload on SQL Server.

Use [books-valid.csv](samples/books-valid.csv) for two copies plus one copy (three physical items). Re-uploading it should create zero copies and skip two existing ISBN rows. [books-mixed-validation.csv](samples/books-mixed-validation.csv) contains one accepted row, Quantity=0 and PublicationYear=0. [books-missing-header.csv](samples/books-missing-header.csv) omits PublicationYear. Use these fictional examples once in a demonstration database; existing ISBNs affect later counts.

## Manager dashboard and exports

Sign in as Manager or Admin and open `/Manager`. Download CSV or Download PDF uses the same reporting service as the dashboard. General availability excludes active loans, active transfers, allocated/ready reservations, eligible waiting requests and inactive physical branches. A copy's physical Available status can therefore differ from its general availability.

Both exports include borrowing statistics, transit and reserved-copy figures, recorded fines, recorded paid/unpaid fines and a loan-level fine audit. The CSV also contains the full item-level inventory health audit. The PDF shows up to the first 200 fine and 200 inventory records, with an explicit truncation note when needed. Event timestamps are displayed in Darwin/ACST; due dates are calendar dates; amounts are AUD. Spreadsheet formula-like text is escaped in the CSV.

Current overdue charges are estimates until a return records the final fine. They are shown separately and are not added to paid-fine revenue. The existing staff Pay fine action changes the recorded payment state; repeated payment does not add a second charge. This is a library payment record, not a bank payment integration.

## Evidence and reconciliation

[Release checks](RELEASE_CHECKS_16OCT.md) distinguish automated fixture results, earlier member screenshots and final Windows results. The synthetic export examples contain 13 fictional copies: three generally available, three reserved Available copies and two in-transit copies. Recorded fines total AUD 9.00: AUD 3.00 paid and AUD 6.00 unpaid. The AUD 1.50 current late-charge estimate is separate.

After running against the final Windows database, compare its dashboard and exports while no one is changing records. Use that database's actual totals; do not expect the synthetic fixture numbers in the shared demonstration database. Retain one screenshot or exported output for each new result; earlier historical screenshots need not be recreated solely for formatting.
