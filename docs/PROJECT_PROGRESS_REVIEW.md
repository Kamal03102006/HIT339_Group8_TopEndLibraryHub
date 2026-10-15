# Group 8 — commit review and completion record

## Current handoffs and release position

All seven feature areas have implementation source across the member branches. The tested lead checkpoint remains **6a69ade**, while the latest master is **deeeaac**. The lead checkpoint and Sandesh's **2ab17fa** are not ancestors of that master revision, so no complete seven-module release is claimed.

Sandesh's report now supplies eight local test cases/captures, his reflection, student number and a partial 12.5-hour retrospective estimate. Corrective work and missing tests remain in [his handoff record](SANDESH_MEMBER_HANDOFF.md). Harsimran supplied an [installation guide](INSTALLATION_GUIDE.md) at **00c3842**; LocalDB setup on his VM failed and the VM check has not passed. Mehakpreet's latest kiosk/API source is in master; final acceptance and her individual records remain to be consolidated.

The [remaining work and finish order](REMAINING_WORK.md) records current owners, dependencies and final checks. Earlier checkpoint entries below are historical.


Reviewed checkpoint: **a1e81908b39b3f76ceb73a0e6b8e0264ed077b37**, pushed to `feature/holds-notifications` on 14 October 2026 at 23:48 Darwin time. This initial review follows the screenshot audit. The latest supplement verifies **6a69ade** after the 15 October 10:06 push report, including patron source/evidence update **70db5b7**. The original review table below remains dated history; module/rubric statuses and owner actions are updated for the patron addition. This is not the final portfolio or a predicted mark.

**The main staff hold, repair, transfer and collection journey is implemented and has substantial observed/reported results. Source for five of the seven expansion areas is integrated. Patron placement is now implemented and has inspected/reported PH evidence. The whole assignment is still unfinished: final kiosk/API/security checks, importer/reports, VM installation, genuine member inputs and the portfolio remain.**

## What was reviewed and verified

| Review | Result | Scope |
| --- | --- | --- |
| Pushed commit | 71 changed paths: 40 application source files, 14 Markdown files, one Word file and 16 image additions. | Compared with its parent dbbc540; all changed source was reviewed with relevant domain/configuration context. |
| Source integrity | All 162 tracked source/configuration assets match the prepared circulation release. | Connects this commit to the source Kamalpreet applied and built. A byte comparison is not another runtime test. |
| Source checks | Git diff whitespace check passed; no unresolved conflict markers found in application code; kiosk JavaScript syntax check passed. | C#/SQL logic was inspected. Build/runtime outcomes are recorded in the Windows test evidence; source checks are a separate verification step. |
| Windows build/database | Build succeeded; all seven migrations Applied=True. | Kamalpreet's report and supplied Get-Migration transcript. Clean-VM installation remains separate. |
| Database integration | Identity link and filtered unique active-loan index are reflected in the context, migration and snapshot. Hold pickup references Branch.Id. ItemStatus values 1–4 are preserved; InTransit is 5. | Source inspection plus supplied migration state. Runtime concurrent checkout is still pending. |
| Screenshot audit | All 21 pushed images accounted for; three original historical notification photos restored, giving 24 indexed images. | Image bytes retained exactly. Original-to-new filenames and honest captions are in [the index](screenshots/README.md). |
| Documentation package | Relative document/image links checked; requirements version 0.5 rendered and every one of its seven pages visually reviewed. | The automatic patch updates evidence records, rather than replacing the whole project. |

The first five hold screenshots predate this commit and are retained. Seven of the 21 images matched already inspected bytes; the other fourteen were inspected individually. No exact duplicate image files were found. `docs/screenshots/README.md` is a text index, not a screenshot, and should remain in that folder.

## Seven required expansion areas

| Required area | Current position | What closes the remaining work | Lead |
| --- | --- | --- | --- |
| Kiosk | Integrated account/scan/checkout/hold source with Identity ownership and two-minute privacy handling. PH01 account/request and PH03 blocking images are inspected; private-data, Finish/Back and idle checks are reported. Final checkout/API/security acceptance remains pending. | Complete successful checkout/protected stock checks, remaining browser routes, JavaScript-disabled expiry, focused POST checks and real keyboard/touch/user feedback. Existing PH images and reports need not be recreated. | Mehakpreet |
| Secure read-only API | API-key middleware and catalogue, available, categories, health/status and code lookup GET actions integrated. | Record missing/wrong/valid key results, projected JSON without patron data, unsupported write rejection and reservation/transit availability. Resolve operating-status interpretation and the proposed filters below. | Mehakpreet |
| Automated simulated notifications | Borrow/due/fine worker, durable log, pickup generation and event/channel uniqueness implemented. Historical borrow/due-soon images and current pickup handover evidence exist. | Complete current fine/date/returned-loan/contact/access checks and loan recovery; retain direct scheduled-trigger evidence. Existing pickup 6 → 6 restart result remains reported. | Kamalpreet |
| Branches/desks/transfers | Three fictional locations/desks, dispatch/receipt and protected reservation handover integrated. Wrong destination/origin receipt and first successful receipt have inspected images. | Verify repeat receipt and repeat-safe setup on the clean VM; complete the final installation manual. | Harsimran |
| Holds/waitlist | Borrowed/damaged FIFO, cancellation/history, repair/transfer/collection and Identity-linked patron placement implemented. PH01 request and PH02 Sky 1 / Asha 2 are inspected; duplicate actions/private-data checks are reported; PH03 GET/UI blocking is inspected. | Complete PH05 forged-input/antiforgery, JavaScript-disabled stale-request and shared checkout/concurrency cases. The staff-only placement gap is closed in source and has documented acceptance evidence. | Kamalpreet, coordinated with Mehakpreet/Harsimran |
| Administrative importer | Source received in Sandesh 2ab17fa; report and eight local captures received. Not integrated into the tested lead release. | Correct quantity/year handling, row feedback and permission evidence; supply raw samples, then integrate and verify. | Sandesh |
| Expanded reports/exports | CSV/PDF summary export source and matching local screenshots received in the Sandesh handoff; final integration pending. | Correct transit/hold-aware stock figures and Darwin dates; add traceable fine detail and non-zero test evidence; reconcile final outputs. | Sandesh |

Five of seven areas having integrated source is approximately **71% by module count only**. Modules differ in size, several acceptance checks remain, and this is not an overall completion percentage or an expected score. Missing pushed handoffs do not establish that a teammate has done no local work.

## Findings to address

1. **The earlier patron placement source gap is addressed.** `/Kiosk/Hold` resolves the signed-in Identity/Borrower link and uses shared staff/patron safeguards. PH01/PH02 placement and FIFO positions have inspected images; duplicate/private-account actions are reported. PH03 supplies membership-blocking images. Complete the remaining POST/security checks; timed pickup expiry remains an optional proposal.
2. **Required importer and reporting breadth remains outstanding in this branch.** Source, sample files, tests and documentation must be received and integrated before claiming all seven modules complete. Fine revenue must be based on recorded payment facts, not estimated accruing charges.
3. **Kiosk privacy needs a complete browser walkthrough.** The patron patch adds Finish/idle handling to signed-in public Kiosk browsing and the Hold route. Finish/Back and idle outcomes are reported, while route-wide browser coverage and a stale request with JavaScript disabled remain separate pending checks. Check graceful expiry as well as denied private access; no comprehensive privacy pass is inferred.
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
| Team contracts | 2 group | Original agreement accepted; date unrecorded | All four accepted the original agreement in the group chat on its creation/sharing day; the calendar date is not recorded. Complete member availability and any changes to responsibilities. |
| Requirements | 4 group | Draft with implemented/tested links | Resolve open policy/API contract decisions and map all final requirements to actual outcomes; patron placement now has PH evidence. |
| Team Approach | 4 group | Draft with real integration examples | Describe the actual branch/model/migration/review/testing approach and record team agreement or changes. |
| Project Plan Overview | 5 group | Working plan/board | Final milestones, dependencies, updated progress/hours, management links and tested marker access. |
| Project Task Allocations | 10 individual | Allocations/evidence partly recorded | Each member's tasks, estimated/actual hours, delivered work, dependencies and commit/issue/test references. |
| Meeting Minutes | 2 group | Written coordination notes; call details pending | Supply actual call dates/participants/decisions/actions to complete genuine minutes. No call recordings are specified in the brief. |
| Strengths and Weaknesses | 8 individual | Kamalpreet reflection and Sandesh member account received; remaining accounts pending | Each member supplies their own strength/limitation, concrete example, action and result. |
| Software meets requirements | 10 group | Five areas integrated; partial acceptance | All seven expansions working together, including the remaining scope gaps and actual integration results. |
| Quality of Source Code | 5 group | Source review/build/migrations supported | Final combined build and appropriate runtime/privacy/repeat/concurrency checks after remaining integration. |
| Value Adding | 5 individual | Real repair/transfer journey and UX defect evidence | Each member identifies their own implemented improvement, beneficiary, reasoning and actual validation/feedback. |

The first eight planning/installation/reflection criteria account for **40/60 marks**. Templates are useful inputs, but they are not completed contracts, meetings, timesheets or personal reflections. Documentation has been updated alongside testing; the final portfolio PDF is still not assembled.

## Who supplies the next work

| Member | Next deliverable |
| --- | --- |
| Kamalpreet | Finish remaining notification/circulation acceptance and meeting records. The patron path is pushed/tested with stated evidence limits, and 19.75 h is a partial session estimate. Coordinate remaining shared requirements, handoffs and original contract references. |
| Mehakpreet | Final linked/unlinked kiosk, checkout, privacy and API results; clarify proposed API filters/status; correct the participant-name inconsistency; confirm the patron guide and real UX results. Supply own hours/reflection/value contribution. |
| Harsimran | Repeat receipt/setup results; final installation instructions tested on a clean VM; any final transfer fixes with branch/commit. Supply own hours/reflection/value contribution. |
| Sandesh | Finish the identified importer/report corrections and missing tests; supply raw samples/exports and confirm the remaining time estimate. His reflection and source handoff are received. Kamalpreet coordinates portfolio assembly. |
| All members | Supply own actual/retrospective task hours, availability and personal reflection/value examples. Existing original contract acceptance is reported by Kamalpreet; confirm any reassigned work. Kamalpreet will provide actual meeting minutes. |

A useful handoff states **branch/commit, route, setup or migration needs, what was personally changed, actual test inputs/results, sample files/screenshots and known limitations**. Message times and commit counts are not billable hours. Retrospective time estimates must be labelled honestly.

## Completion order

1. Apply the review/evidence patch and commit the documentation/image organisation. Application source is unchanged, so earlier workflow tests do not need repeating solely because pictures were renamed.
2. Close patron placement and finish kiosk/API/privacy verification against the combined code. Confirm the exact operating-status/filter contract. Run one purposeful simultaneous reception/kiosk checkout check.
3. Receive/import Sandesh's source and verify the required importer/report areas; finish Harsimran's repeat-action and VM evidence. Finish the distinct remaining notification checks.
4. Compile actual team inputs into the contracts, task/hours table, minutes and individual reflections/value accounts. Update requirement and test status from real outcomes.
5. Identify the final integrated repository revision. Assemble the portfolio PDF with all Part A artifacts, patron guide, integration diagrams and user journeys. Inspect the exported PDF and verify marker read access before submission.

Internal target: **15 October 2026**. Official deadline: **16 October 2026, 23:59 Darwin**. If a trial service is used for assessment access, keep it available for two weeks after submission; none is currently confirmed.

Source records: [assessment checklist](ASSESSMENT_CHECKLIST.md), [group work](GROUP_WORK_RECORD.md), [circulation tests](CIRCULATION_INTEGRATION_TEST_RECORD.md), [notification tests](NOTIFICATION_TEST_RECORD.md), [screenshot audit](SCREENSHOT_AUDIT.md), [requirements](HIT339_Group8_Requirements_Draft.docx), supplied Group Project brief and Blackboard rubric images. Source checks, Windows test results and screenshot observations are recorded separately. [AI acknowledgement](AI_ACKNOWLEDGEMENT.md).

## Documentation checkpoint after this review

The review/evidence package was pushed at **107f643**, 15 October 00:59 Darwin. All 235 tracked files match the prepared package; all 24 images are present and application source is unchanged from a1e8190. The next documentation update populates known task/evidence rows, records the two supplied written coordination messages and drafts Kamalpreet's reflection in GROUP_WORK_RECORD.md. Phone-call facts, task-level durations and the other members' hours still require member input; the final portfolio remains incomplete. A later [partial retrospective session record](KAMALPREET_TIME_RECONSTRUCTION.md) gives Kamalpreet a reconstructed subtotal of 19.75 hours through 15 October 09:47 Darwin, using her two-hour cutoff. It does not close the complete-hours or final individual-evidence requirements.

## Latest verified patron/member checkpoint — 15 October

**6a69ade** contains **70db5b7**, the patron feature, evidence and Pkcs 9.0.21 update, followed by the member-record commit. All prepared combined files and the project dependency set match the pushed release; PH01/PH02 were additional uploads and have now been inspected. All **28 images** are present. The supplied 15 October clean Windows build and audit remain the runtime/dependency results; this documentation verification did not repeat the Windows runtime tests.

PH01 visibly confirms successful Sky placement and Waiting position 1 at Palmerston; PH02 shows Sky #6 first and Asha #7 second, both awaiting repair. PH03 blocks inactive/unlinked access. Duplicate/private-data and Finish/Back/idle actions remain reported, and the unrun checks remain pending. Kamalpreet's reflection now includes the completed patron and package-fix work. The 19.75 h session subtotal is partial, with its original 09:47 cutoff.

The earlier fetch was limited to the lead branch. A full-heads fetch subsequently confirmed Sandesh's 2ab17fa and PR #7. His report and eight captures have now been reviewed; importer/report code and evidence are received, with corrections still open. Current handoffs and final actions are listed above. No overall completion percentage or predicted mark is assigned.
