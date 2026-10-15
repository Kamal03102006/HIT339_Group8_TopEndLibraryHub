# Group 8 — commit review and completion record

Reviewed checkpoint: **a1e81908b39b3f76ceb73a0e6b8e0264ed077b37**, pushed to `feature/holds-notifications` on 14 October 2026 at 23:48 Darwin time. This review follows the screenshot audit. It records the current position; it is not the final portfolio or a predicted mark.

**The main staff hold, repair, transfer and collection journey is implemented and has substantial observed/reported results. Source for five of the seven expansion areas is integrated. The whole assignment is still unfinished: patron hold placement, final kiosk/API checks, importer/reports, VM installation and genuine team documentation remain.**

## What was reviewed and verified

| Review | Result | Scope |
| --- | --- | --- |
| Pushed commit | 71 changed paths: 40 application source files, 14 Markdown files, one Word file and 16 image additions. | Compared with its parent dbbc540; all changed source was reviewed with relevant domain/configuration context. |
| Source integrity | All 162 tracked source/configuration assets match the prepared circulation release. | Connects this commit to the source Kamalpreet applied and built. A byte comparison is not another runtime test. |
| Source checks | Git diff whitespace check passed; no unresolved conflict markers found in application code; kiosk JavaScript syntax check passed. | C#/SQL logic was inspected. This review environment has no .NET SDK or SQL Server, so it did not independently build or execute the application. |
| Windows build/database | Build succeeded; all seven migrations Applied=True. | Kamalpreet's report and supplied Get-Migration transcript. Clean-VM installation remains separate. |
| Database integration | Identity link and filtered unique active-loan index are reflected in the context, migration and snapshot. Hold pickup references Branch.Id. ItemStatus values 1–4 are preserved; InTransit is 5. | Source inspection plus supplied migration state. Runtime concurrent checkout is still pending. |
| Screenshot audit | All 21 pushed images accounted for; three original historical notification photos restored, giving 24 indexed images. | Image bytes retained exactly. Original-to-new filenames and honest captions are in [the index](screenshots/README.md). |
| Documentation package | Relative document/image links checked; requirements version 0.5 rendered and every one of its seven pages visually reviewed. | The automatic patch updates evidence records, rather than replacing the whole project. |

The first five hold screenshots predate this commit and are retained. Seven of the 21 images matched already inspected bytes; the other fourteen were inspected individually. No exact duplicate image files were found. `docs/screenshots/README.md` is a text index, not a screenshot, and should remain in that folder.

## Seven required expansion areas

| Required area | Current position | What closes the remaining work | Lead |
| --- | --- | --- | --- |
| Kiosk | Integrated account/scan/checkout source, touch controls, Identity ownership and two-minute privacy handling. Windows combined build confirmed; final runtime acceptance pending. | Demonstrate linked and unlinked accounts, a successful checkout and protected stock rejection, Finish/Back/idle reset, and keyboard/touch use. | Mehakpreet |
| Secure read-only API | API-key middleware and catalogue, available, categories, health/status and code lookup GET actions integrated. | Record missing/wrong/valid key results, projected JSON without patron data, unsupported write rejection and reservation/transit availability. Resolve operating-status interpretation and the proposed filters below. | Mehakpreet |
| Automated simulated notifications | Borrow/due/fine worker, durable log, pickup generation and event/channel uniqueness implemented. Historical borrow/due-soon images and current pickup handover evidence exist. | Complete current fine/date/returned-loan/contact/access checks and loan recovery; retain direct scheduled-trigger evidence. Existing pickup 6 → 6 restart result remains reported. | Kamalpreet |
| Branches/desks/transfers | Three fictional locations/desks, dispatch/receipt and protected reservation handover integrated. Wrong destination/origin receipt and first successful receipt have inspected images. | Verify repeat receipt and repeat-safe setup on the clean VM; complete the final installation manual. | Harsimran |
| Holds/waitlist | Borrowed/damaged FIFO, duplicate prevention, cancellation history, repair allocation and next-family handover implemented. Staff journey has observed/reported results. | **Add a patron-facing hold-placement path using the signed-in Identity/Borrower link, or obtain explicit lecturer acceptance of staff-assisted placement.** Also verify inactive eligibility and shared kiosk/concurrency cases. | Kamalpreet, coordinated with Mehakpreet/Harsimran |
| Administrative importer | No importer implementation or sample-file handoff is present in this reviewed branch. | Receive Sandesh's branch/commit, runnable route, sample CSVs and actual results; integrate and verify valid import, invalid rows, duplicates and administrative access. | Sandesh |
| Expanded reports/exports | Existing Assignment 2 dashboard is present. The required expansion and exports are not present in this reviewed branch. | Receive and integrate borrowing, fine revenue audit and inventory health reports with exports; reconcile known data, filters, payment facts and permissions. | Sandesh |

Five of seven areas having integrated source is approximately **71% by module count only**. Modules differ in size, several acceptance checks remain, and this is not an overall completion percentage or an expected score. Missing pushed handoffs do not establish that a teammate has done no local work.

## Findings to address

1. **Patron hold placement is a requirement alignment gap.** The brief says patrons can place a hold; `HoldsController` permits Admin/Reception only, and the kiosk currently displays progress. A membership number alone must not grant account ownership. Reuse the verified Identity link and shared item/queue safeguards for patron placement. Staff cancellation can remain the first implementation; timed pickup expiry is an optional proposal, distinct from placement.
2. **Required importer and reporting breadth remains outstanding in this branch.** Source, sample files, tests and documentation must be received and integrated before claiming all seven modules complete. Fine revenue must be based on recorded payment facts, not estimated accruing charges.
3. **Kiosk privacy needs a complete browser walkthrough.** Account/Scan have Finish and idle scripts; the public Kiosk index currently has no such control. Verify returning to public browsing while signed in, idle expiry, Back and a server request with JavaScript disabled. Check graceful expiry behaviour as well as denial of private data. No privacy runtime pass is inferred from the code.
4. **API proposals exceed the current actions.** Requirements A01 proposes branch/category-filtered results, while the API search/available actions currently accept search/limit or limit only. Implement the agreed filters or explicitly record a reviewed requirements adjustment. `/status` is system/database health; confirm whether branch opening status is also expected. These are open decisions, not silently completed capabilities.
5. **The notification recheck drops filters.** `CheckNow` redirects to an unfiltered Index and its form carries no current filters. This can make pickup totals appear mixed with loan confirmations. Until corrected, reapply the item, message-type and channel filters after a manual check. The actual cause of Kamalpreet's earlier selection sequence was not independently observed. No filter-preservation code change is included in this documentation patch.
6. **The final source link needs to identify the complete version.** `master` was at an older checkpoint in the fetched references. Use the current feature-branch link during development; merge or otherwise identify the final seven-module revision for submission. Test repository and project-board read access without the owner's session.
7. **Historical member reports need final confirmation.** Mehakpreet's feedback document uses Navdeep in its participant table and Princedeep elsewhere. Obtain her correction and final integrated test evidence; retain historical claims as member-reported, rather than treating them as independently verified passes.

The review found shared per-item transactions, common availability checks, protected physical location changes, FIFO ordering, post-commit notification hooks and database uniqueness guards in the implemented source. These support the tested journey; they do not substitute for the remaining runtime demonstrations.

## Completed evidence and its limits

Observed images establish duplicate rejection; Asha/Noah initial queue order; cancellation retaining history; Noah Ready and later Collected at Darwin; initial damaged FIFO; borrowing rejection; Asha allocated with zero premature pickup records; wrong destination/origin-receipt errors; Palmerston receipt; correct Asha pickup Email/SMS; and Asha Collected with Noah Ready.

Kamalpreet additionally reported correct reserved-member/repeated-checkout behaviour, one active loan, transit borrowing rejection, a later NeedsRepair return keeping the new request waiting, repair increasing pickup history from four to six, and restart retaining six. These remain accurately labelled reports where the exact result is not pictured.

Two captures require narrow captions: the filled transit checkout form shows no rejection, and the new Asha #5 card says **Awaiting return**, before the damaged return. Their filenames/captions now reflect that. The completed repair/restart checks do not need to be recreated solely to obtain more pictures; no screenshot was requested for the last restart count. Future meaningful checks can produce additional evidence naturally.

## All eleven rubric criteria

The supplied rubric titles/maximums total 60: **37 group and 23 individual marks**. Detailed HD-band descriptors were not supplied. The actions below follow the titles and full brief; they are not fabricated marking-band quotations.

| Criterion | Marks | Current status | Remaining concrete evidence |
| --- | ---: | --- | --- |
| Installation and Running instructions | 5 group | Partial | Final clone/prerequisite/database/seed/location instructions followed successfully on a VM, with results and corrections. |
| Team contracts | 2 group | Acceptance reported by the leader | Kamalpreet states all four accepted the original draft; original date/time and reply references are unconfirmed. Retain that distinction and complete real availability/changes. |
| Requirements | 4 group | Draft with implemented/tested links | Resolve the open decisions and patron-placement/API scope gaps; map final requirements to actual outcomes. |
| Team Approach | 4 group | Draft with real integration examples | Describe the actual branch/model/migration/review/testing approach and record team agreement or changes. |
| Project Plan Overview | 5 group | Working plan/board | Final milestones, dependencies, updated progress/hours, management links and tested marker access. |
| Project Task Allocations | 10 individual | Allocations/evidence partly recorded | Each member's tasks, estimated/actual hours, delivered work, dependencies and commit/issue/test references. |
| Meeting Minutes | 2 group | Written coordination notes; call details pending | Supply actual call dates/participants/decisions/actions to complete genuine minutes. No call recordings are specified in the brief. |
| Strengths and Weaknesses | 8 individual | Kamalpreet draft prepared; other member accounts pending | Review the lead's draft; each other member supplies their own strength/limitation, concrete example, action and result. |
| Software meets requirements | 10 group | Five areas integrated; partial acceptance | All seven expansions working together, including the remaining scope gaps and actual integration results. |
| Quality of Source Code | 5 group | Source review/build/migrations supported | Final combined build and appropriate runtime/privacy/repeat/concurrency checks after remaining integration. |
| Value Adding | 5 individual | Real repair/transfer journey and UX defect evidence | Each member identifies their own implemented improvement, beneficiary, reasoning and actual validation/feedback. |

The first eight planning/installation/reflection criteria account for **40/60 marks**. Templates are useful inputs, but they are not completed contracts, meetings, timesheets or personal reflections. Documentation has been updated alongside testing; the final portfolio PDF is still not assembled.

## Who supplies the next work

| Member | Next deliverable |
| --- | --- |
| Kamalpreet | Close patron hold placement with the kiosk owner; finish remaining notification/eligibility/integration checks; coordinate handoffs and maintain the final requirement/evidence records. Supply genuine own hours and reflection. |
| Mehakpreet | Final linked/unlinked kiosk, checkout, privacy and API results; clarify proposed API filters/status; correct the participant-name inconsistency; confirm the patron guide and real UX results. Supply own hours/reflection/value contribution. |
| Harsimran | Repeat receipt/setup results; final installation instructions tested on a clean VM; any final transfer fixes with branch/commit. Supply own hours/reflection/value contribution. |
| Sandesh | Importer plus samples and tests; expanded reports/exports with reconciled payment/stock data; a branch/commit handoff. Assemble the portfolio from verified artifacts and supplied team inputs. Supply own hours/reflection/value contribution. |
| All members | Supply own actual/retrospective task hours, availability and personal reflection/value examples. Existing original contract acceptance is reported by Kamalpreet; confirm any reassigned work. Kamalpreet will provide actual meeting minutes. |

A useful handoff states **branch/commit, route, setup or migration needs, what was personally changed, actual test inputs/results, sample files/screenshots and known limitations**. Message times and commit counts are not billable hours. Retrospective time estimates must be labelled honestly.

## Completion order

1. Apply the review/evidence patch and commit the documentation/image organisation. Application source is unchanged, so earlier workflow tests do not need repeating solely because pictures were renamed.
2. Close patron placement and finish kiosk/API/privacy verification against the combined code. Confirm the exact operating-status/filter contract. Run one purposeful simultaneous reception/kiosk checkout check.
3. Receive/import Sandesh's source and verify the required importer/report areas; finish Harsimran's repeat-action and VM evidence. Finish the distinct remaining notification checks.
4. Compile actual team inputs into the contracts, task/hours table, minutes and individual reflections/value accounts. Update requirement and test status from real outcomes.
5. Identify the final integrated repository revision. Assemble the portfolio PDF with all Part A artifacts, patron guide, integration diagrams and user journeys. Inspect the exported PDF and verify marker read access before submission.

Internal target: **15 October 2026**. Official deadline: **16 October 2026, 23:59 Darwin**. If a trial service is used for assessment access, keep it available for two weeks after submission; none is currently confirmed.

Source records: [assessment checklist](ASSESSMENT_CHECKLIST.md), [group work](GROUP_WORK_RECORD.md), [circulation tests](CIRCULATION_INTEGRATION_TEST_RECORD.md), [notification tests](NOTIFICATION_TEST_RECORD.md), [screenshot audit](SCREENSHOT_AUDIT.md), [requirements](HIT339_Group8_Requirements_Draft.docx), supplied Group Project brief and Blackboard rubric images. AI assistance compiled this review from source and the supplied observations; genuine member experiences and unrun test outcomes have not been invented.

## Documentation checkpoint after this review

The review/evidence package was pushed at **107f643**, 15 October 00:59 Darwin. All 235 tracked files match the prepared package; all 24 images are present and application source is unchanged from a1e8190. The next documentation update populates known task/evidence rows, records the two supplied written coordination messages and drafts Kamalpreet's reflection in GROUP_WORK_RECORD.md. Phone-call facts, task-level durations and the other members' hours still require member input; the final portfolio remains incomplete. A later [partial retrospective session record](KAMALPREET_TIME_RECONSTRUCTION.md) gives Kamalpreet a reconstructed subtotal of 19.75 hours through 15 October 09:47 Darwin, using her two-hour cutoff. It does not close the complete-hours or final individual-evidence requirements.
