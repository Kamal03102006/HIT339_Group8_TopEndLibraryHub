# Sandesh member handoff

Member: Sandesh Bhattarai S390535. Component: CSV book importer and manager reporting exports. Original component source: **2ab17fa**, branch **feature/sandesh-csv-reports**, PR **#7**. Report supplied on 15 October 2026. Later corrections were reviewed through **da0f483** on 16 October. The combined release adds shared-rule reporting and production-code tests; those later integration changes are recorded separately from the member's own work.

## Received test evidence

The member documents eight local test cases as passed. The original report's eight test captures are retained byte-for-byte below. These are component results; final integration, non-zero fines and access-denial cases remain separate.

| Case | Reported result or visible state | Capture | Evidence limit |
| --- | --- | --- | --- |
| TC01 | 100 imported / 0 skipped | [sd_tc01_bulk_100_imported.png](screenshots/sandesh/sd_tc01_bulk_100_imported.png) | The success message is visible; input file contents are supplied by the member. |
| TC02 | 0 imported / 100 skipped | [sd_tc02_duplicate_100_skipped.png](screenshots/sandesh/sd_tc02_duplicate_100_skipped.png) | The skipped count is visible; duplicate-ISBN cause is member-reported. |
| TC03 | 0 imported / 1 skipped | [sd_tc03_one_row_skipped.png](screenshots/sandesh/sd_tc03_one_row_skipped.png) | The skipped count is visible; Quantity=0 is member-reported. |
| TC04 | Required-column validation | [sd_tc04_missing_headers.png](screenshots/sandesh/sd_tc04_missing_headers.png) | The missing-header message is visible. |
| TC05 | BK-0102 found in the catalogue | [sd_tc05_catalogue_bk0102_year_zero.png](screenshots/sandesh/sd_tc05_catalogue_bk0102_year_zero.png) | Searchability is visible; Year 0 is visible in this historical capture. Current validation rejects it; this old image is not a corrected-version pass. |
| TC06 | CSV summary opened | [sd_tc06_csv_summary.png](screenshots/sandesh/sd_tc06_csv_summary.png) | 104 total items, 102 available and zero fine totals are visible. |
| TC07 | PDF summary opened | [sd_tc07_pdf_summary.png](screenshots/sandesh/sd_tc07_pdf_summary.png) | Visible figures agree with the supplied CSV summary; this is not a non-zero fine test. |
| TC08 | Branch SQL result | [sd_tc08_branch_sql_result.png](screenshots/sandesh/sd_tc08_branch_sql_result.png) | Read-only query output shows BK-0102 at BranchId 1, Darwin Hub. |

## Final integration verification

The earlier one-copy/Year=0 defects have source corrections in the combined release, with automated quantity, invalid-year and duplicate-import checks. Dashboard/CSV/PDF share availability and fine definitions. Run the final Windows upload, route-access and real SQL tests in [release checks](RELEASE_CHECKS_16OCT.md). Keep TC01–TC08 as historical component results; do not relabel them as new-version evidence. Representative CSV samples, actual synthetic exports and operating instructions are included.

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

## Later source corrections and combined release

Sandesh subsequently added Quantity/PublicationYear handling, transit counts, Darwin dates and fine audit details in CSV/PDF. His later commits also removed a duplicate BranchId property and duplicate branch/transfer creation from the consolidated migration. These are received contributions. The integration retains the lead database's canonical seven-migration history instead of adding a second Holds/NotificationLogs creation.

The combined release now uses the same availability predicate for API, catalogue and reporting; includes fine and inventory audit rows; separates recorded fines from current estimates; and shows import errors per row. Automated checks call production import, reporting, fine and payment code and generate labelled synthetic exports. See [release checks](RELEASE_CHECKS_16OCT.md) for results and the Windows/SQL checks still pending. The original eight captures above remain historical component evidence.

## Time record

The supplied dated log totals 12.5 hours and is partial. Its final testing/documentation duration and later correction hours have not been supplied. Commit counts and elapsed time are not used to invent those hours. Sandesh only needs to confirm genuine additional task durations and complete the targeted release checks available in his environment; he does not need to rewrite his whole report.
