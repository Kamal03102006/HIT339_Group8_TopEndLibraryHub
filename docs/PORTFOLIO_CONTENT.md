# TopEndLibraryHub

HIT339 Distributed Development | Group 8 | 16 October 2026

:::toc

# 1. A connected library service

We expanded Kamalpreet's Assignment 2 application into seven access channels and operational modules. Our main example addresses a practical problem: a family should keep its place in a waiting list while a toy is repaired or moved to the branch where it can be collected.

A repaired copy at Darwin is not yet ready at Palmerston. A physical Available copy can still be promised to another patron. If the catalogue, kiosk and report disagree, staff and patrons receive different answers. We connected the queue, item condition, physical location, checkout and simulated messages so that those stages agree.

| Addition | Practical value | Example checked |
| --- | --- | --- |
| Kiosk/private account | Scan and borrow, or follow only the current member's requests. | Identity-linked checkout, inactive/unlinked guards and Sky's account. |
| Secure API | Another application can read catalogue projections without write/account access. | Ten HTTPS checks for keys, endpoints, validation and rejected POST. |
| Simulated messages | Explain events and avoid repeated channel records. | No early pickup notice; Email/SMS after destination arrival. |
| Branches/desks/transfers | Trace origin, destination, dispatch and receipt. | Wrong-branch receipt and transit checkout rejected. |
| Holds/queue | Keep a fair request through return, repair and movement. | Asha/Noah order, cancellation and next-member promotion. |
| Importer | Add physical copies in bulk with validation feedback. | Two rows create three copies; repeated upload creates none. |
| Manager exports | Explain borrowing, fines and inventory outside the dashboard. | Matching CSV/PDF and a non-zero recorded payment. |

The value we demonstrate is consistency across this journey: patrons retain their queue position and staff can explain when and where collection is possible. We retained one MVC deployment and database to support the horizontal expansion required by the brief.

# 2. Our agreement, approach and plan

:::include GROUP_WORK_RECORD.md

## Rubric coverage

:::include ASSESSMENT_CHECKLIST.md

# 3. Requirements and decisions

:::include REQUIREMENTS.md

# 4. Discussion minutes

:::include MEETING_MINUTES.md

# 5. Integration and user journeys

## Shared services and data

:::architecture

Figure 1. Browser channels, secure API and administrative import share one data model. Circulation, holds and reports use common eligibility; notifications are simulated durable records.

| Shared concern | Choice | Purpose |
| --- | --- | --- |
| Availability | CirculationRules.GenerallyAvailable and reservation queries. | Do not offer a promised/moving copy as general stock. |
| Checkout/return | CirculationService with per-item SQL transaction/lock. | Kiosk and staff protect the same loan, hold and transfer state. |
| Private ownership | Borrower.IdentityUserId from the authenticated login. | A posted member ID or coincidental email must not choose another account. |
| Queue/readiness | Shared placement/workflow services. | Keep FIFO and pickup location through repair and movement. |
| Event uniqueness | EventKey/Channel unique index and background scan. | A repeated check must not multiply the same event/channel. |
| Reports | One ManagerReportingService snapshot. | Dashboard/CSV/PDF need the same eligibility and fine definitions. |
| Schema | Seven canonical migrations and explicit Identity key lengths. | Avoid duplicate-table creation and model/schema drift. |

## Hold progression and physical movement

:::holdstates

Figure 2. InTransit is an item/transfer state, not a HoldStatus. A request can stay Allocated while its copy travels. Destination receipt permits Ready; collection creates the loan and fulfils the request.

## Journey A - repaired toy to family pickup

| Stage | User action | System response | Evidence |
| --- | --- | --- | --- |
| Discover | Asha finds an unavailable toy and selects pickup at Palmerston. | Explain status and preserve the item code through login. | Catalogue/patron source and PH captures. |
| Join | Asha and Noah request the same borrowed/damaged copy. | One active request per member/item and stable FIFO. | H01-H03 and C07. |
| Prepare | Staff repair it at Darwin. | Allocate Asha, keep Noah waiting, send no early pickup notice. | C08 allocation/zero-record captures. |
| Move | Dispatch to Palmerston. | Prevent a different destination, origin receipt and transit checkout. | C09/C10 and transfer component tests. |
| Receive | PAL-D01 receives it. | Change physical branch, make Asha Ready and log two pickup channels. | C11 receipt/readiness/messages. |
| Collect | Asha checks out at the pickup branch. | Fulfil her request and create one active loan while Noah waits. | C12 and real SQL rightful-owner test. |
| Reuse | Return a usable copy. | Record any fine and promote the next eligible request. | C12 next-ready and real SQL return test. |

## Journey B - an auditable management decision

An administrator selects an active branch and imports a CSV. Quantity=2 creates two separate codes. Repeating existing ISBN rows skips them. A manager can then see each copy's branch, physical condition, reservation restriction and general availability in the inventory audit.

A current overdue estimate is separate from a recorded fine. When a returned loan's AUD 27 fine is recorded as paid, total recorded fines stay 27, paid moves from 0 to 27 and unpaid from 27 to 0. The payment timestamp is retained and the current estimate stays 9. Each figure can be traced to the loan that caused it.

# 6. Individual work, hours and reflections

Each member describes their contribution, working hours, strengths, difficulties and learning. Retrospective estimates are labelled in the hours tables. The original reports and source history support these accounts.

## 6.1 Kamalpreet - S389008

:::include individual/KAMALPREET.md

### My working-session details

:::include KAMALPREET_TIME_SUMMARY.md

## 6.2 Harsimran Singh - S388232

:::include individual/HARSIMRAN_SINGH.md

## 6.3 Mehakpreet Kaur - S390303

:::include individual/MEHAKPREET_KAUR.md

### My task hours

:::include individual/MEHAKPREET_HOURS_REVIEW.md

## 6.4 Sandesh Bhattarai - S390535

:::include individual/SANDESH_BHATTARAI.md

### My source history

:::include individual/SANDESH_COMMIT_HISTORY.md

# 7. UX feedback and refinements

:::include UX_FEEDBACK_RECORD.md

We use the green library theme and direct task names such as Collection, Self service, Join waiting list and My library account. The interface explains why a copy cannot be borrowed and what happens next. The informal reviewers requested no further changes; the internal input/filter/button observations above led to concrete improvements. We do not present these as a formal accessibility or measured usability study.

# 8. Installation and running guide

:::include INSTALLATION_GUIDE.md

## Our actual installation result

The combined Windows build and all 33 SQL-enabled tests passed at b79faf3. This verifies the application and database operations in that environment. The required separate VM installation did not reach a working application: LocalDB failed when SQL Server VSS Writer could not start, and the administrator attempt failed too. The cause of the installer failure was not established.

We also resolved a locked generated AssemblyInfo.cs file in the OneDrive project by stopping app/build servers, removing generated bin/obj and rebuilding. Updating Pkcs 9.0.4 to 9.0.21 removed the supplied build warnings; the package audit reported no vulnerable packages from its configured sources at that check. We would trial the VM guide early and check resources/permissions before the final integration period next time.

# 9. Patron search and self-service guide

:::include ../Documentation/PatronUserGuide.md

## Understanding your request

| Message | Meaning | Next action |
| --- | --- | --- |
| Available | No general borrowing restriction identified. | Check the physical branch and use kiosk/staff checkout. |
| On hold | The copy has a reservation or eligible request. | Do not assume it is available to everyone. |
| Waiting / Awaiting repair or return | The request is queued and the copy is not ready. | Keep the request and follow progress. |
| Allocated | A copy is assigned but may still need to travel. | Wait for readiness at the chosen branch. |
| In transit | Dispatched but not yet received. | Wait for receipt; borrowing is blocked. |
| Ready for collection | The reserved copy is at the selected pickup branch. | Sign in there and collect your own allocation. |
| Inactive / not linked | This login cannot create a loan or request. | Ask authorised staff to check membership status/link. |

:::figure docs/screenshots/ph01_sky_waiting_palmerston.png | Figure 3. Sky's account confirms their own request at Palmerston. This 15 October capture shows Waiting, not readiness or collection.

# 10. Verification and source quality

:::include RELEASE_CHECKS_16OCT.md

## Test depth

Our fixtures call production FinePolicy, payment, importer, reporting/export, API-key and filter code. Cases cover invalid quantities/years, separate physical copies, duplicate imports, reserved/transit stock, CSV formula-like text, report totals and repeat-payment timestamps. A fixed clock crosses Darwin midnight. A separate model/script check finds no pending model changes and checks the seven migration names and repeated shared-table creation.

EF InMemory does not establish SQL translation, foreign keys, transactions or locking. The real SQL cases create isolated databases and apply the canonical migrations before checking rightful Ready collection with a waiting follower, two simultaneous checkouts producing one loan and late return recording an AUD 6 fine while promoting the next request. All three ran in the 33-test Windows suite.

The synthetic report uses 13 fictional items, three generally available, three reserved Available and two in transit; AUD 9 recorded fines split 3 paid/6 unpaid, plus a separate 1.50 estimate. The actual Windows dataset below has nine items. We keep these and older member captures with 104/107 items separate.

## Actual Windows import and report audit

:::include test-evidence/windows-browser-and-reports-16oct.md

## API contract

:::include test-evidence/windows-api-result-16oct.md

## Private access and its limits

:::include test-evidence/windows-browser-access-16oct.md

# 11. Selected evidence

The selected figures document queue order, cancellation, allocation, destination readiness, import validation and reporting. The repository evidence index links each capture to its test and retains complementary images where a result spans two screens.

:::figure docs/screenshots/h02_fifo_queue_details.png | Figure 4. Asha is first and Noah second for borrowed TY-0001. The complementary overview is retained in the repository.

:::figure docs/screenshots/h03_cancelled_hold_queue_overview.png | Figure 5. Cancellation promotes Noah; the complementary history capture retains Asha's Cancelled record.

:::figure docs/screenshots/c08_asha_allocated_palmerston.png | Figure 6. Repair allocates Asha for Palmerston while Noah waits. Allocation at origin does not mean readiness at destination.

:::figure docs/screenshots/c08_no_early_pickup_notifications.png | Figure 7. The filtered TY-9001 pickup history has zero records before arrival. The messages are simulated Email/SMS records.

:::figure docs/screenshots/c11_asha_ready_noah_waiting_palmerston.png | Figure 8. Destination receipt makes Asha Ready while Noah still waits. The receipt and two channel messages are separately indexed.

:::figure docs/screenshots/harsimran/hs02_wrong_branch_receipt.png | Figure 9. The 13 October component check rejects receipt at the wrong branch.

:::figure docs/screenshots/release-16oct/r01_csv_three_copies_imported.png | Figure 10. Actual Windows import: three physical copies, two accepted rows, zero skips. Repeating the sample created none and skipped two ISBN rows.

:::figure docs/screenshots/release-16oct/r04_csv_missing_year_header.png | Figure 11. The actual importer rejects missing PublicationYear. The mixed sample/capture and accepted inventory are retained separately.

:::figure docs/screenshots/release-16oct/r05_manager_dashboard_before_payment.png | Figure 12. Nine items, four generally available, two active/overdue loans and one reserved Available copy. Fine totals below this crop are verified from actual exports. The duplicate button pair visible here was removed in the final source.

# 12. Evaluation, sources and acknowledgement

## Evaluation and limitations

The combined release supports the seven modules and the connected repair-to-pickup journey. Shared circulation rules keep availability consistent across the catalogue, kiosk, API and manager outputs. The actual Windows tests and exported audit rows provide evidence for those behaviours.

The separate VM installation failed during LocalDB setup. External Email/SMS delivery and bank payment processing are outside the implementation: notifications are simulated records, and fine payment is recorded by library staff. The browser security checks cover the specific scenarios listed in section 10.

Earlier shared data agreements, smaller integration batches and an early VM trial would have reduced the final integration pressure. A current project board and daily task/time logs would also have made progress easier to track.

## Source index

| Source | Purpose |
| --- | --- |
| Group Project.pdf | Supplied two-page brief: Parts A/B, VM check and PDF/repository deliverables. |
| Supplied rubric | Eleven criteria and weights totalling 60. |
| Rohan's assignment-two announcement | Requirements questions, marketing, feedback acted on, breadth and depth as design prompts. |
| Public Git repository and project board | Source history, tasks and marking read access. |
| docs/REQUIREMENTS.md | Seven-module requirements, acceptance checks and shared business rules. |
| docs/PROJECT_TASK_REGISTER.md | Full task ownership, completion status and evidence at the integration review. |
| docs/MEETING_MINUTES.md | Discussion decisions, responsibilities and action closeout. |
| docs/individual/ | Four personal accounts, hours and verified source history. |
| docs/member-reports/ and Documentation/ | Original member reports and kiosk/API/patron material. |
| docs/screenshots/README.md | Evidence captions and original filename mapping. |
| docs/test-evidence/ | Linux logs/TRX, Windows records, actual exports, payment/API results and labelled synthetic outputs. |

## My acknowledgement of assistance

:::include AI_ACKNOWLEDGEMENT.md
