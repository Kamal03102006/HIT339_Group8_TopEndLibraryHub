# Sandesh member handoff

Member: Sandesh Bhattarai S390535. Component: CSV book importer and manager reporting exports. Source: **2ab17fa**, branch **feature/sandesh-csv-reports**, PR **#7**. Report supplied on 15 October 2026.

## Received test evidence

The member documents eight local test cases as passed. The original report's eight test captures are retained byte-for-byte below. These are component results; final integration, non-zero fines and access-denial cases remain separate.

| Case | Reported result or visible state | Capture | Evidence limit |
| --- | --- | --- | --- |
| TC01 | 100 imported / 0 skipped | [sd_tc01_bulk_100_imported.png](screenshots/sandesh/sd_tc01_bulk_100_imported.png) | The success message is visible; input file contents are supplied by the member. |
| TC02 | 0 imported / 100 skipped | [sd_tc02_duplicate_100_skipped.png](screenshots/sandesh/sd_tc02_duplicate_100_skipped.png) | The skipped count is visible; duplicate-ISBN cause is member-reported. |
| TC03 | 0 imported / 1 skipped | [sd_tc03_one_row_skipped.png](screenshots/sandesh/sd_tc03_one_row_skipped.png) | The skipped count is visible; Quantity=0 is member-reported. |
| TC04 | Required-column validation | [sd_tc04_missing_headers.png](screenshots/sandesh/sd_tc04_missing_headers.png) | The missing-header message is visible. |
| TC05 | BK-0102 found in the catalogue | [sd_tc05_catalogue_bk0102_year_zero.png](screenshots/sandesh/sd_tc05_catalogue_bk0102_year_zero.png) | Searchability is visible; Year 0 is also visible and remains a data-quality defect. |
| TC06 | CSV summary opened | [sd_tc06_csv_summary.png](screenshots/sandesh/sd_tc06_csv_summary.png) | 104 total items, 102 available and zero fine totals are visible. |
| TC07 | PDF summary opened | [sd_tc07_pdf_summary.png](screenshots/sandesh/sd_tc07_pdf_summary.png) | Visible figures agree with the supplied CSV summary; this is not a non-zero fine test. |
| TC08 | Branch SQL result | [sd_tc08_branch_sql_result.png](screenshots/sandesh/sd_tc08_branch_sql_result.png) | Read-only query output shows BK-0102 at BranchId 1, Darwin Hub. |

## Open corrections and tests

The current importer validates Quantity but constructs one Book per accepted row. PublicationYear is omitted, leaving 0 despite the model's 1000–2100 range. Correct these and retest. Improve row-specific feedback. Complete report coverage for transit and hold-aware availability, use shared Darwin dates and include traceable fine details.

Add actual quantity/year, unauthorised-access, non-zero unpaid/paid-fine and final integrated stock tests. Keep TC01–TC08 as historical evidence for 2ab17fa; append new outcomes with the corrected revision. Supply the raw CSV samples, actual CSV/PDF exports and module operating instructions.

## Member supplied retrospective time estimate

| Date | Task | Estimated hours |
| --- | --- | ---: |
| 12 October | CSV preparation and requirements review | 2.0 |
| 14 October | Project setup and troubleshooting | 1.5 |
| 14 October | CSV importer development | 3.0 |
| 14 October | Initial importer testing | 1.0 |
| 14 October | Manager dashboard and CSV reporting | 2.0 |
| 14 October | PDF export and navigation | 2.0 |
| 15 October | Version control and coordination | 1.0 |
| 15 October | Additional testing and documentation | To confirm |

The numerical subtotal is **12.5 hours**, explicitly supplied as retrospective estimates rather than a recorded timesheet. The last duration and any corrections remain for Sandesh to supply. The subtotal must not be added again to the individual task rows.

## Individual reflection supplied by Sandesh

Contribution and application improvement
The primary thing I did was the CSV book importer and the CSV/PDF exporter for the manager dashboard. The importer enables an administrator to add many book records from a file instead of entering them one by one. It checks for required fields, stops duplicate ISBN records, produces unique library codes, and attaches imported books to a branch. I have put links in the interface so that it’s easy to find these tools. The manager downloads make it easy to evaluate and maintain as records, borrowing and fine summary numbers.
A strength, with an example
When I was troubleshooting unknown code, I was persistent. The importer was originally written by me in my previous solo project, however the group application used BookGenre instead of Genre, and required both a properly formatted LibraryCode and a BranchId. I checked out the Book, Item, Branch and database context classes, fixed the importer, then showed I could successfully import 100 books.
A difficulty, response and outcome
The most difficult component was adapting my work to a broader programme that was being updated by other people in the organisation. I had troubles with the .NET SDK, missing package references, build errors. I went through the bugs one by one; I tried the features in the browser and checked the branch assignment with a read only Local DB query for book BK-0102. The record was associated with Darwin Hub (BranchId 1).
What I learned and what I would improve
I learned more about ASP.NET Core MVC, Entity Framework Core, CSV validation, QuestPDF reporting, and git feature branches. Next time I would review group project models before coding, create smaller and earlier changes, track my actual time daily and design negative and integration tests from the beginning. I would also work with the group on reporting definitions before considering the FinePaid indication as a payment audit.
