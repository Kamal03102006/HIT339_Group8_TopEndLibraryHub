# Sandesh Bhattarai - individual contribution and reflection

Student number: S390535 | CSV book importer and manager dashboard/CSV/PDF exports

## My contribution

I adapted the importer and exports to the shared Book, Item, Branch and loan models. My branch is feature/sandesh-csv-reports and my pull request is #7. My initial implementation is 2ab17fa, code corrections run through da0f483 and final documentation is 97e53b6. The integration retains these histories and records later shared-rule corrections separately.

## My reflection

### Contribution and application improvement

The primary thing I did was the CSV book importer and the CSV/PDF exporter for the manager dashboard. The importer enables an administrator to add many book records from a file instead of entering them one by one. It checks for required fields, stops duplicate ISBN records, produces unique library codes, and attaches imported books to a branch. I have put links in the interface so that it’s easy to find these tools. The manager downloads make it easy to evaluate and maintain as records, borrowing and fine summary numbers.

### A strength, with an example

When I was troubleshooting unknown code, I was persistent. The importer was originally written by me in my previous solo project, however the group application used BookGenre instead of Genre, and required both a properly formatted LibraryCode and a BranchId. I checked out the Book, Item, Branch and database context classes, fixed the importer, then showed I could successfully import 100 books.

### A difficulty, response and outcome

The most difficult component was adapting my work to a broader programme that was being updated by other people in the organisation. I had troubles with the .NET SDK, missing package references, build errors. I went through the bugs one by one; I tried the features in the browser and checked the branch assignment with a read only Local DB query for book BK-0102. The record was associated with Darwin Hub (BranchId 1).

### What I learned and what I would improve

I learned more about ASP.NET Core MVC, Entity Framework Core, CSV validation, QuestPDF reporting, and git feature branches. Next time I would review group project models before coding, create smaller and earlier changes, track my actual time daily and design negative and integration tests from the beginning. I would also work with the group on reporting definitions before considering the FinePaid indication as a payment audit.

## My strengths, weakness and value

My strength was persisting through unfamiliar model/package errors and checking the actual source and browser/SQL results. My weakness was bringing the combined work together late. The earlier component captures could not establish corrected Quantity/year behaviour or a non-zero fine audit, so the final release needed focused integration checks.

My importer reduces repeated manual entry by creating separately coded physical books from a CSV row. My manager exports make borrowing, fine and inventory information portable. The combined report explains why a physical Available copy can still be reserved and separates recorded payments from current late-charge estimates.

## My hours

My updated report supplies a **35-hour retrospective task estimate**, replacing the earlier 12.5-hour partial record. I count it once; these are task estimates, not a contemporaneous daily timesheet.

| Task | Estimated hours |
| --- | ---: |
| Requirements review and coordination | 2 |
| CSV importer development | 5 |
| Quantity, ISBN and year improvements | 3 |
| Manager dashboard/calculations | 4 |
| CSV/PDF exports | 4 |
| Database/migration troubleshooting | 4 |
| Manual functional/access tests | 4 |
| Automated xUnit examples | 3 |
| Git/PR/conflict resolution | 3 |
| Screenshots and report | 3 |
| Total | 35 |

## My evidence and learning

My final report has ten captures and reported cases covering bulk/repeat/header import, branch assignment, restricted access and a transfer scenario. One historical image shows Year 0; current validation rejects that value. My eight xUnit examples are component arithmetic/status checks, separate from the combined 33 production/SQL tests.

The later actual Windows import/report/payment results are recorded separately. I would review the common models earlier, integrate smaller changes, agree reporting definitions first and keep daily actual hours next time.
