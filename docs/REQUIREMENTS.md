# Our requirements and decision notes

We used the project brief to define the required scope. We expanded the existing .NET 10 MVC application through seven new modules/channels while retaining Books, Music and Toys. Our demonstration follows an unavailable toy through a fair queue, repair, branch transfer and collection. The decisions below connect those requirements to the implemented behaviour and acceptance evidence.

## Users and access

| User | Need | Access |
| --- | --- | --- |
| Public patron | Search title/code/keyword and understand physical location/availability. | /Catalogue and /Kiosk |
| Linked library member | Scan, borrow, see only their own account and choose pickup. | /Kiosk/Scan, /Account and /Hold |
| Reception | Loans, requests, repairs and branch movement at a selected desk. | /Loans, /Holds, /StaffLocation, /Transfers |
| Admin | Maintain collection and bulk-import book copies. | Administration and /CsvImport |
| Manager | Borrowing, fine and inventory review/export. | /Manager and export actions |
| External developer | Protected read-only public catalogue projections. | /api/catalogue |

## Seven module requirements and acceptance

| ID | Requirement | Owner | Acceptance evidence |
| --- | --- | --- | --- |
| FR01 | Simplified kiosk with code scanning/typing, Identity-linked account, eligible 14-day checkout and private loans/requests. Reject inactive/unlinked membership and another member's allocation. | Mehakpreet; shared integration | Component checkout/account, PH01-PH03 and Windows rightful-owner SQL case. Fresh account request after Finish asks for login. |
| FR02 | Secure GET-only catalogue, available copies, categories and operating status. Private API key, validated filters, no member account fields. | Mehakpreet | All ten HTTPS checks plus production middleware/filter tests. |
| FR03 | Automatically simulate Email/SMS for borrowed, due-soon and overdue/fine events; record ready-for-pickup events without duplicates. | Kamalpreet | Historical loan/due-soon captures, C08 zero early notice and C11 destination messages, written recheck/restart results. |
| FR04 | Active branches/desks and physical copy locations; dispatch at origin, receive at destination, prevent transit checkout and retain audit history. | Harsimran | Seven historical component captures and C09-C11 integration journey. |
| FR05 | Holds on borrowed/damaged copies, FIFO by creation time/ID, one active member/item request and one allocation; notify only when ready at selected pickup. | Kamalpreet | H01-H03, C07-C13, PH01-PH03 and real SQL return/next-member cases. |
| FR06 | Admin CSV utility with required headers and row validation; Quantity creates separate uniquely coded books at an active branch; repeated ISBN import creates none. | Sandesh | Production tests and actual 3-copy/2-row import, repeat 0/2 skipped, mixed 1/2 skipped and missing-year rejection. |
| FR07 | Manager/Admin CSV/PDF borrowing, fine revenue audit and inventory health exports using dashboard definitions. | Sandesh; shared integration | Actual dashboard/CSV/PDF reconcile; AUD 27 payment transition verified. |

## Shared rules

| Rule | Implementation and check |
| --- | --- |
| Physical Available is not sufficient for general borrowing. | Shared eligibility excludes active loan/transfer, usable reservation/eligible queue and inactive branch. Catalogue, kiosk, API and reports agree. |
| A rightful Ready member may collect while another waits. | CirculationService and Windows SQL ReadyOwner case. |
| No two active loans or allocations for one copy. | Per-item SQL transaction/lock and filtered unique indexes; simultaneous checkout creates one loan. |
| Pickup is a real branch; transit preserves previous enum values. | PickupBranchId references Branch.Id; InTransit=5 keeps values 1-4. |
| Inventory edit does not silently move a copy. | Create uses a valid branch; ordinary edits preserve it; transfers handle movement. |
| Repair away from pickup means Allocated, not Ready. | Destination receipt triggers readiness/messages; zero early notices are captured. |
| Current login determines private membership. | Explicit IdentityUserId, not a posted borrower ID or automatic email matching. |
| Date/fine definitions are consistent. | UTC event storage, Darwin calendar calculations, AUD 1.50 per overdue day, fixed-clock tests across midnight. |
| Recorded revenue and current estimates differ. | Returned-loan fines/paid state separate from active-loan estimate; actual payment leaves recorded total and estimate unchanged. |
| One compatible migration history. | Seven canonical migrations, no pending model changes/duplicate table creation; fresh SQL execution in isolated tests. |

## Inputs and quality requirements

| Condition | Implementation/evidence |
| --- | --- |
| Admin-only import; Manager/Admin reports; Admin/Reception branch operations. | Roles and antiforgery metadata; selected signed-out/Patron denials. Forged-POST runtime checks remain unrun. |
| Shared kiosk privacy. | Finish/sign-out, no-store and two-minute idle handling. Normal idle and fresh-request logout confirmed; history-cache/server-only expiry not separately tested. |
| CSV at most 5 MB. | Title, Author, ISBN, Category, Quantity, PublicationYear required; Publisher optional. Quantity 1-100; year 1000 through earlier of 2100 and next year. Row results capped at 200 displayed with full counts. |
| API filters. | limit 1-100, category Book/Music/Toy, active branchId; search at most 100 characters. Invalid filter 400, missing item 404, POST 405, missing configured key 503. |
| Auditable exports. | Shared snapshot, AUD/ACST, full CSV inventory/fine rows; PDF first 200 fine/200 inventory rows with truncation note; formula-like CSV text escaped. |
| Same installation guide at each location, tested on VM. | Common guide provided; successful Windows/SQL run. VM LocalDB setup failed, so successful VM verification is incomplete. |
| Readable management/source access. | Public GitHub repository and board are readable without authentication; the task register records delivery and evidence. |

## Questions we resolved

| Question | Our decision and reason |
| --- | --- |
| Which baseline? | Kamalpreet's Assignment 2 codebase, with one set of common models. |
| Can an email identify a borrower? | Use an explicit Identity link so coincidental email equality cannot choose account ownership. |
| When does off-site repair trigger pickup? | After destination receipt, so a patron is not sent to an unavailable copy. |
| Does Quantity mean one record with a count? | Create separately coded physical Book records; copies from the row may share an ISBN. |
| Is a current estimate fine revenue? | No; keep it separate until a return records a fine and payment is recorded. |
| What does operating status mean? | Service/database health and configured branch activity. We have no live opening-hours data. |
| How should competing shared files be merged? | Retain one service/migration chain, review compatibility and test the integrated behaviour. |

Integration corrections addressed membership input, notification filter retention, rightful pickup collection, importer Quantity/year handling and manager availability definitions. We removed duplicate export buttons after inspecting the actual browser screen. Section 10 records the completed verification and its limits.
