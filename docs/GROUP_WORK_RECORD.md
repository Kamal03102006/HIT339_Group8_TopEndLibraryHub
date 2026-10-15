# HIT339 Group 8 working governance record

## Current update after member handoffs

Sandesh source **2ab17fa** and his testing report are received. Eight test captures, his reflection and a partial 12.5-hour estimate are recorded in [the member handoff](SANDESH_MEMBER_HANDOFF.md). Harsimran's guide **00c3842** records an attempted but unpassed VM setup. Mehakpreet's latest source has merged into master **deeeaac**; the lead's tested release remains **6a69ade** and final integration is pending. See [remaining work](REMAINING_WORK.md) for current actions. The original agreement was accepted by all four on its creation/sharing day; the calendar date is unrecorded.

## Latest verified progress — 15 October patron/member push

- Latest push is verified at **6a69ade**, containing patron source/evidence/package update **70db5b7** and the member records. The prepared combined files match the pushed release, package references match the fixed configuration, and all **28** images are present. PH01 visibly establishes Sky's Waiting request at Palmerston; PH02 establishes Sky 1 / Asha 2. Earlier dbbc540/a1e8190/107f643 checkpoints and their evidence remain history.
- Screenshots verify duplicate hold rejection, Asha/Noah FIFO positions and cancellation promoting Noah to position 1. These are three UI tests on borrowed TY-0001 at Darwin, not complete circulation acceptance.
- Kamalpreet applied the 54-file circulation patch, including Mehakpreet's 3b925ee source and shared reception/kiosk/repair/transfer guards. Windows build succeeded; the supplied migration list shows all seven migrations Applied=True. The combined source/documentation checkpoint is now verified pushed at a1e8190.
- The earlier 24 images and four patron-hold captures are matched and indexed. Observed results include Noah Ready/Collected, damaged FIFO, Asha allocation and zero early messages, protected transfer errors, destination receipt, correct pickup channels and collection/next-family cards. Later damaged-return/repair and pickup restart 6 → 6 remain tester reports. In-transit checkout rejection and one-active-loan/repeated-checkout checks remain reported where their exact result is not pictured. Kiosk/API/privacy, concurrency, inactive eligibility and repeat receipt remain distinct checks.
- Detailed expected steps are maintained in CIRCULATION_INTEGRATION_TEST_RECORD.md. Kamalpreet supplies actual outcomes; documentation is maintained from those reports.
- Import/report source and Sandesh records are received; code corrections, final integration, successful VM verification, remaining member records, original agreement calendar date and actual minutes remain outstanding. Earlier dated progress entries below are historical.


Version 1.1, documentation and member handoffs updated on 15 October 2026; reviewed source baseline is 6a69ade. The partial time record retains its 09:47 cutoff. Includes the team agreement record, Kamalpreet's individual reflection, known task/evidence rows and a partial retrospective session estimate. Individual task splits and other members' hours remain unrecorded. The patron addition extends the circulation source reviewed at a1e8190 without a new schema migration. Earlier timestamped entries are historical. This contains the accepted agreement record, working arrangements, verified progress and records awaiting member input. It is not completed meeting minutes or the final portfolio.

## Team agreement and working arrangements

Group members: Kamalpreet, Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai.

**Acceptance status:** All four members accepted the original team agreement in the group chat on the day it was created and shared. The original calendar date is not recorded. The responsibilities and working arrangements are listed below.

1. Confirm each member's responsibilities and available working times before treating the allocation as agreed.
2. Keep development in named Git feature branches; include meaningful commits and a short description of how to run and verify the change.
3. Agree shared database models, migrations and borrowing/hold/transfer rules before changes that affect another module. Coordinate migration generation to avoid conflicting snapshots.
4. Keep source, acceptance evidence and documentation updated together. Each member supplies real hours, observations and design decisions. The portfolio coordinator assembles the written records from those inputs.
5. Review integration changes with a teammate. Check the common borrower, item, location and availability rules across kiosk, reception, API and reports.
6. Raise blockers with the affected member and leader promptly; record the dependency and an agreed revised action.
7. Aim to finish implementation, testing and documentation by **15 October**, leaving submission checks before the official **16 October, 23:59 Darwin** deadline.
8. Use AI as an aid to reasoning and implementation. Check outputs, adapt them to agreed requirements, test them and follow any course disclosure rules. Do not invent hours, meetings, client feedback or individual contributions.

| Member | Proposed responsibility | Accepted? Date/reference | Actual availability |
| --- | --- | --- | --- |
| Kamalpreet | Lead coordination; requirements; holds; simulated notifications; integration evidence | Accepted in the group chat on creation/sharing day; calendar date not recorded. | To enter |
| Harsimran Singh | Branches, reception desks, transfers; installation manual and VM test | Accepted in the group chat on creation/sharing day; calendar date not recorded. | To enter |
| Mehakpreet Kaur | Kiosk and read-only API; patron guide and UX evidence | Accepted in the group chat on creation/sharing day; calendar date not recorded. | To enter |
| Sandesh Bhattarai | CSV import and expanded reports/exports; module documentation; Kamalpreet coordinates portfolio assembly | Accepted in the group chat on creation/sharing day; calendar date not recorded. | To enter |

## Proposed approach to completing the project

Use the selected Assignment 2 MVC application as the baseline and add the seven required horizontal modules. Confirm requirements and interfaces, implement in feature branches, demonstrate each module against its acceptance checks, then integrate and test a shared patron/staff journey. Update the technical guide, patron guide, evidence record and task log after each meaningful change. Reserve 15 October for final integrated testing, VM instructions, document checks and portfolio assembly.

The fictional family-lending focus is books, music and toys across Darwin Hub, Palmerston Hub and Casuarina Hub. These are proposed demonstration locations, not claims about real libraries. Prioritize breadth and coherent integration before optional enhancements or deep architectural refactoring.

## Requirements decisions and notes

The [requirements draft](HIT339_Group8_Requirements_Draft.docx) contains acceptance checks and open questions. Record actual answers here; a proposal is not a client or team approval.

| Decision | Current proposal / question to resolve | Agreed answer, respondent and date |
| --- | --- | --- |
| D01 / Q1 | Confirm allocation, availability and the family-lending demonstration focus. | Pending |
| D02 / Q2 | Proposed use of existing ASP.NET Core Identity with a unique optional link from each Borrower to its patron login; resolve private account access from the authenticated user ID. Scanner input and kiosk session reset/timeout need verification. | Mehakpreet requested coordination on 11 October. Her branch at `3b925ee` now includes the optional unique Identity link, its migration and an account action that resolves Borrower from the authenticated user ID. The applied circulation patch now uses shared checkout; kiosk ownership, checkout and privacy runtime checks remain pending. Team acceptance and runtime integration checks remain to be recorded. |
| D03 / Q3 | Read-only API consumer and usage contract need confirmation. Mehakpreet has implemented configurable API-key middleware and available-items, categories and operating-status endpoints. Avoid patron personal data. | Source reviewed through `3b925ee` on 14 October. GET catalogue, available, categories and system/database status actions exist; final hold/transfer availability rules, authentication results and consumer contract remain to be verified. |
| D04 / Q4 | Confirm loan period, $1.50 daily fine proposal, Darwin date rules and any restrictions for unpaid fines. | Pending |
| D05 / Q5 | Confirm physical-copy FIFO, cancellation/expiry and transfer policy. The brief permits patron placement. The initial staff-only gap is now addressed by Identity-linked /Kiosk/Hold and the shared placement service in 70db5b7. | Open policy answers still require review. Patron placement and shared FIFO/duplicates have inspected/reported results in PATRON_HOLD_TEST_RECORD.md; POST security and concurrency remain pending. Pickup expiry remains a separate optional proposal. |
| D06 / Q6 | Confirm CSV error handling, required fields and whether both CSV and PDF report exports are required. | Pending |
| D07 / integration | Connect Hold.PickupBranchId to Branch.Id. Item.BranchId records physical location; a transfer keeps the origin until destination receipt. Preserve hold allocation during and after transfer. | Harsimran supplied the branch contract on 12 October. Source verified on 14 October in `master` at `99aa527`: branches/desks, transfers and `ItemStatus.InTransit = 5` exist. Kamalpreet generated the restricted Hold pickup-branch foreign key and index on 14 October; the supplied migration was reviewed. The applied patch includes receipt/notification integration, all seven migrations show Applied=True, and the main handover is tester-reported tested. Final kiosk/concurrency and VM checks remain pending. |

For each changed decision, record the reason, affected requirement IDs, affected members and commit/test references. Keep the draft requirements consistent with the actual agreement.

## Billable task breakdown

Estimates below are **planning placeholders in person-hours**, to be revised and accepted by each owner. They are not hours already worked. Baseline September hours in `DEVELOPMENT_LOG.md` are Assignment 2 history and must not be counted as group-project time.

| Task | Proposed owner | Est. hours | Actual hours | Evidence / dependency |
| --- | --- | ---: | --- | --- |
| Agree requirements answers, roles and contract | All; Kamalpreet coordinates | 2 | Not recorded | D01–D07; acceptance record |
| Notification implementation and migration | Kamalpreet | 4 | Not recorded | Commits `a8a5dbe`, `1e052e7`, `26f1fbd`, `bcf6844`; implementation exists |
| Complete notification acceptance evidence | Kamalpreet | 2 | Not recorded | Notification test record; several checks pending |
| Hold foundation and queue constraints | Kamalpreet | 3 | Not recorded | Commits `683b508` through `24094c6`; foundation exists |
| Hold creation, cancellation and fair allocation | Kamalpreet | 4 | Not recorded | Creation, eligible queue display and waiting cancellation are implemented in the desk batch; three guided tests were reported completed on 14 October. Return/repair allocation is implemented and reported tested in the circulation record. |
| Hold return/repair/transfer integration and tests | Kamalpreet + Harsimran | 3 | Not recorded | Main flow reported tested and supplied captures now matched; remaining kiosk/concurrency/inactive/repeat checks are separate |
| Branch/desk model and repeat-safe seed | Harsimran | 3 | Not recorded | Shared schema agreement |
| Transfer dispatch/receipt and invalid-action tests | Harsimran | 4 | Not recorded | Branches, item availability and holds |
| Installation manual and clean VM verification | Harsimran | 3 | Not recorded | Final integrated version and branch selection |
| Kiosk account and checkout journey | Mehakpreet | 4 | Not recorded | Shared checkout/hold/transfer eligibility |
| Secure read-only API and request examples | Mehakpreet | 3 | Not recorded | Available item and location/status definitions |
| Patron search guide and real UX feedback/retest | Mehakpreet | 2 | Not recorded | Final screens and an actual test participant |
| CSV validation/import and representative sample files | Sandesh | 4 | Not recorded | Agreed metadata and branch identifiers |
| Manager reports/exports and revenue audit checks | Sandesh | 4 | Not recorded | Agreed payment facts and final inventory statuses |
| Cross-module integration and test evidence | All | 5 | Not recorded | All seven working modules; split actual time by person |
| Portfolio, integration diagrams and journey maps | Sandesh coordinates; all contribute | 4 | Not recorded | Complete artifacts and actual test/UX evidence |
| README, access check and final submission review | All; Kamalpreet coordinates | 2 | Not recorded | Final branch/revision, links and exported PDF |

Adjust the breakdown if work is reassigned or an estimate changes. For shared tasks, the estimate is combined person-hours; record each person's actual contribution separately. Completed coding still needs its actual hours entered by the person who did it.

### Actual time record — partial lead-session estimate and missing task durations

Known task dates and delivery evidence are entered below. At 09:47 Darwin on 15 October, Kamalpreet selected a retrospective method: retain chat-session gaps of two hours or less and exclude gaps longer than two hours. Recoverable messages and dated activity checkpoints produce a **partial subtotal of approximately 19.75 hours**; see [the reconstruction and coverage limits](KAMALPREET_TIME_RECONSTRUCTION.md). This is elapsed-session evidence, not a complete active-time or billable timesheet. No numeric actual task hours are inferred from commit counts or the planning estimates, and no arbitrary split of the session subtotal between tasks is made.

| Date / evidence checkpoint | Member | Task | Actual hours / retrospective estimate | Work performed | Commit or evidence |
| --- | --- | --- | --- | --- | --- |
| 4 October | Kamalpreet | Requirements and proposed allocation | Awaiting duration | Added the group requirements draft for review. | f6c75b9 |
| By 11 October; actual work dates to confirm | Kamalpreet | Holds/notification foundation | Awaiting duration | Applied foundation/migration work; demonstrated the notification page, loan-confirmation and due-soon simulations. | bcf6844; N-OBS-01–03 |
| 14 October | Kamalpreet | Branch/transfer integration and merge resolution | Awaiting duration | Integrated the branch/transfer contribution and resolved shared-file merge issues. | 74668e1 |
| 14 October | Kamalpreet | Hold desk, pickup FK and membership validation | Awaiting duration | Applied the desk/interface changes, generated the pickup relationship migration, changed the case-sensitive membership pattern and ran the initial hold checks. | dbbc540; H01–H03; UX-01 |
| 14 October | Kamalpreet | Shared circulation integration | Awaiting duration | Applied the combined circulation/kiosk/API source, built on Windows and checked all seven applied migrations. | a1e8190; setup transcript |
| 14 October | Kamalpreet | Return/repair/transfer/collection acceptance | Awaiting duration | Ran guided staff journeys, reported guard/count results and captured the supplied screenshots. | C01–C13; reported pickup C19 6 → 6 |
| 14–15 October | Kamalpreet | Evidence organisation and documentation update | Awaiting duration | Supplied/pushed screenshots, applied the review patch, checked image count/progress-file existence and committed the organised records. | 107f643; 24 indexed images |
| Dates to confirm | Kamalpreet | Team coordination | Awaiting duration | Requested progress and coordinated shared Identity/branch/hold interfaces with the module owners. | Supplied messages; D02/D07; written notes below |
| To enter | Harsimran Singh | Own assigned/delivered tasks | Member supplies duration | Record actual work and contribution, including any installation/VM work. | Member supplies references |
| To enter | Mehakpreet Kaur | Own assigned/delivered tasks | Member supplies duration | Record actual kiosk/API/guide work and final results. | Member supplies references |
| To enter | Sandesh Bhattarai | Own assigned/delivered tasks | Member supplies duration | Record actual importer/report/portfolio work and handoff. | Member supplies references |

Avoid counting one work period twice when coding, testing and documenting overlapped. Exclude breaks, unrelated activities and Assignment 2 baseline work. For shared activities, each person records their own time. Active time reading, applying and checking assisted instructions is part of the member's work; an elapsed chat window is not a reliable total.

The lead-session subtotal is recorded once; it must not be repeated against every task above. Its earliest recovered checkpoint is 3 October at 07:44 Darwin and its cutoff is 15 October at 09:47. Short breaks and time waiting within the retained windows may be included by the selected rule. Missing or offline sessions remain unrecorded, not zero. Other members provide their own durations. Exact task-level splits and any corrections can be entered from member recollection or fuller records without counting overlapping periods twice.

If an estimate of past time is necessary, label it a retrospective estimate and explain its basis; do not present it as a contemporaneous timesheet.

## Verified progress and blockers

| Date | Work or event | Evidence | Limit / next action |
| --- | --- | --- | --- |
| 4 October | Group requirements draft added to repository | Commit `f6c75b9` | Review and answer open questions |
| Observed by 11 October | Holds foundation, notification implementation and migration pushed | Feature branch through `bcf6844` | Complete hold workflow and remaining runtime checks |
| 11 October | User sent Harsimran a request for branch/desk model progress and a pushed branch name | Kamalpreet reported sending the drafted message | No reply or member branch supplied in this audit; this is not evidence that he has done no work |
| 11 October | Notifications page and borrowed/due-soon messages demonstrated | [Notification test record](NOTIFICATION_TEST_RECORD.md) | Fine, restart, permissions and integration evidence pending |
| 11 October | Requirements audit, documentation gaps and evidence records prepared | [Assessment checklist](ASSESSMENT_CHECKLIST.md) and this working record | Import into the Windows project, review and commit; obtain real team inputs |
| 11 October | Working assessment/governance/test records committed by user | Commit `35767fc` on `feature/holds-notifications` | Draft governance fields still need genuine team input; these are not final portfolio artifacts |
| 11 October | Mehakpreet asked to agree Identity login and secure linkage to Borrower; a detailed reply was prepared | User supplied her message; later source checked at `73a8792` | Record actual team agreement and implement/test the link; account page remains a placeholder in pushed code |
| 12 October | Harsimran reported pushed branch foundation and confirmed pickup branch reference and planned transit value | User supplied his message; verified branch foundation commit was `c5eb71c` | Local combined foundation files were prepared; those older merge resolutions need updating for subsequent transfers |
| 14 October | Branch, reception desk, location, transfer dispatch/receipt and item location handling reviewed in updated source | `origin/master` `99aa527`; `origin/harsimran-branches` `75a1a9e` | Build/runtime evidence, hold protection on receipt, final installation guide and VM evidence pending |
| 14 October | User is importing the proposed green interface into existing MVC views and stylesheet | User stated current UI work; three source files prepared | No successful build or runtime screenshot supplied yet |
| 14 October | Full brief re-read and assessment checklist updated | [Assessment checklist](ASSESSMENT_CHECKLIST.md) | Full rubric labels/HD descriptors still absent; final portfolio, diagrams, journey maps and team evidence need completion |
| 14 October, 09:53 Darwin | User supplied two Blackboard rubric screenshots showing all eleven category titles and maximum marks | Screenshot titles/marks matched to the earlier pasted group/individual labels | Rubric now mapped: 37 group marks and 23 individual marks; expanded performance descriptors are not shown |

## Management links and marker access

- Repository: https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub
- Planning board: https://github.com/users/Kamal03102006/projects/1/views/2
- Current implemented feature branch: `feature/holds-notifications`; final submission branch/revision: **pending**.
- Marker read access: **not yet tested**. Check the final links in a signed-out/private session; document the result and provide read access details if necessary.
- Trial service used: **none confirmed**. If used, record expiry and ensure access for two weeks after submission.

## Meeting and discussion records — actual call details awaited

On 15 October Kamalpreet referred to phone discussions without recordings. Their dates, participants, decisions and actions have not yet been supplied here. The provided brief asks for meeting minutes and does not specify call recordings. A genuine call can be written up retrospectively from the participants' recollection; identify that basis and retain the actual facts.

The following notes summarise the two member messages supplied in this workflow. They are **written coordination records**, not invented telephone meetings or proof that all four members attended a discussion. Dates below identify when the messages were provided in the project discussion; original sent times were not preserved.

| Record | Date context / channel | People evidenced | Topic and requested action | Subsequent source/evidence |
| --- | --- | --- | --- | --- |
| CO-01 / D02 | Supplied 11 October; written message | Mehakpreet's message to the lead | Asked to confirm ASP.NET Identity sign-in and verified Borrower linkage; stated she would coordinate shared-model changes and respect hold/transfer restrictions. | Identity link and shared checkout present in combined source. A complete reply/agreement or a phone-call record is not inferred. |
| CO-02 / D07 | Supplied 12 October; written message | Harsimran's message to Kamalpreet | Reported branch/desk/item-location foundation; proposed InTransit=5 while preserving 1–4; requested transit exclusion and PickupBranchId → Branch.Id coordination before the next migration. | Those contracts appear in the integrated source; all seven migrations were later reported applied. This does not establish a call, attendance or billable duration. |

### Phone-call minutes intake

Kamalpreet confirmed on 15 October that she will supply the actual meeting minutes. The member request is therefore for task hours and individual reflection/value evidence; no call is invented from a written chat.

For each genuine call, Kamalpreet can give the date (approximate if necessary), who participated, what was discussed/agreed and who was to do what. The coordinator will turn those facts into the record below. Unremembered details stay unspecified; do not copy written-message topics into a call record unless they were actually discussed on that call.

- Meeting date/time and channel: to enter.
- Attendees and apologies: to enter.
- Purpose and progress discussed: to enter.
- Decisions and requirement references: to enter.
- Actions: record each action, owner and due date.
- Blockers and next check-in: to enter.
- Recorder and attendee corrections/acceptance: to enter.

## Team strengths and weaknesses — member input required

The rubric allocates **8 individual marks** to Strengths and Weaknesses. Each member must provide their own evidence-based reflection; the table below is an intake structure rather than a finished reflection.

| Member | Strength with an example | Limitation or learning need | Agreed response |
| --- | --- | --- | --- |
| Kamalpreet | Coordinates module dependencies and reports concrete integration and UX problems; examples include pickup-branch coordination and the membership capitalisation defect. | Developing independence with unfamiliar Git/EF integration and written evidence; the need for documentation support was explicitly reported. | Uses consistent test inputs, checks the displayed results and keeps the test evidence connected to the relevant changes. |
| Harsimran Singh | To enter | To enter | To enter |
| Mehakpreet Kaur | To enter | To enter | To enter |
| Sandesh Bhattarai | To enter | To enter | To enter |

Use concrete examples such as a tested workflow, database review, guide written or issue resolved. Choose actions that respond to actual limitations, such as a paired review or an earlier integration checkpoint.

### Kamalpreet individual reflection

Member: Kamalpreet, S389008. Role: group lead, holds and simulated notifications, with shared integration work.

We used my Assignment 2 library application as the starting point for Group 8. I coordinated the module allocation and shared interfaces, applied changes in Visual Studio and helped integrate the holds and notification workflows with Mehakpreet's kiosk/API work and Harsimran's branches and transfers. Keeping those contributions connected became a significant part of my role because each module affected the same borrowers, items and locations.

One strength I showed was persistence in checking problems that appeared during testing. When a membership number looked uppercase on the registration screen but was rejected, I noticed that its underlying input was still lowercase. I applied a validation pattern that accepts either case while retaining consistent uppercase storage. This was a small issue, but it mattered because a person entering a valid membership number should not be blocked by the way the form displays it.

The repair and transfer tests helped me check the application as a connected workflow. For a damaged toy at Darwin with pickup at Palmerston, repair produced an allocation without an early pickup message. The request became ready after the destination received the item. I then checked collection, the next family's turn and a later return requiring repair. These cases showed why item condition, physical location and reservation status needed to be checked separately.

My main difficulty was unfamiliar Git and Entity Framework integration. I needed guidance to resolve shared-file changes and understand which changes to commit. I also found some similar-looking test steps confusing. When six notification rows included both loan confirmations and pickup messages, I needed to apply the pickup-only filter before comparing the expected count. I responded by checking the exact input and message type, reporting the displayed result and preserving the existing history while investigating it.

On 15 October I applied the patron hold form so an active linked member could request a borrowed or damaged item through their own account. The screenshots show Sky waiting first at Palmerston and Asha second in the shared queue. I checked duplicate requests and private progress, tested inactive/unlinked access and checked sign-out behaviour. I also resolved a build-file lock and updated the Pkcs dependency; the subsequent Windows build and package audit succeeded.

The value of my work is that a family's place can survive repair and a branch transfer, with notification when the copy reaches the chosen pickup location. The account also gives the family a way to follow its own request. For future projects, I would agree shared interfaces earlier, integrate smaller batches and capture the relevant result immediately. I would also make the distinction between implemented code and completed acceptance checks clearer from the start. The remaining tests and group deliverables are recorded in the project checklist.

Evidence: H01–H03, C01–C13, reported pickup C19 and PH01–PH04; UX-01 and UX-03; source commits 74668e1, dbbc540, a1e8190 and 70db5b7. My time record contains a partial retrospective estimate of 19.75 hours through 09:47 on 15 October, using my selected two-hour session cutoff. [AI acknowledgement](AI_ACKNOWLEDGEMENT.md).

## UX and client-value evidence template

- Participant and date: record with consent; use an anonymous identifier if appropriate.
- Real task attempted: for example, find an available toy and understand where to collect it.
- What happened: record observation and feedback accurately.
- Design decision/change: explain the connection to the observed problem.
- Commit/screen evidence and retest: record the actual outcome.

The family repair-to-pickup journey is a proposed product story. Its value should be demonstrated in the working product and real feedback, not by claiming unsupported novelty.

## Individual evidence pack — each member completes their own entries

The confirmed individual categories are **Project Task Allocations (10), Strengths and Weaknesses (8), and Value Adding (5)**. These total **23 individual marks**. The prompts below are our evidence plan; detailed marking-band descriptors were not visible in the screenshots.

### Project Task Allocations — 10 marks

For each assigned task, record the owner, estimated hours, actual hours or clearly labelled retrospective estimate, work delivered, current status, dependencies and commit/PR/issue/test references. Explain any reassignment and the actual contribution to shared work. Keep a member's record consistent with the Git history and group task table.

### Strengths and Weaknesses — 8 marks

- Member name and role: to enter.
- Strength demonstrated by a specific task or decision: to enter from actual work.
- Evidence and effect on the project: to enter.
- Real limitation, difficulty or learning need: to enter.
- Action taken, support sought or practice changed: to enter.
- Actual outcome and remaining improvement: to enter.

Use personal examples rather than generic statements about teamwork. The portfolio coordinator can format entries from a member's spoken or written account; another person's experience is not inferred from their job allocation.

### Value Adding — 5 marks

- Member name and implemented contribution: to enter.
- Patron, staff member or developer problem addressed: to enter.
- Why the chosen change helps that user: to enter.
- Own design/implementation contribution and commit references: to enter.
- Actual test, observation or feedback: to enter.
- Resulting improvement or remaining limitation: to enter.

Potential examples, only after implementation and verification: Kamalpreet's fair repair/hold/pickup workflow; Harsimran's reliable transfer/location handover; Mehakpreet's usable and private kiosk journey; Sandesh's actionable importer validation or reconciled reports. These are candidate evidence directions, not claims that work is complete. The green theme can support a usability contribution when its benefit is tested; appearance alone does not establish a particular mark.

## 14 October afternoon implementation update

- Kamalpreet reported the merge/build/commit/push batch complete at 16:38 Darwin. The pushed commit `74668e1` was verified and its three shared files match the prepared merge source. Detailed integrated runtime results are still pending.
- Staff-assisted hold creation and waiting-list cancellation/display, a real pickup-branch relationship, and the green interface are prepared in a single patch against that revision. Their import, migration, build and runtime checks are not yet recorded.
- FIFO eligibility excludes inactive memberships and inactive pickup branches while preserving the original request timestamp. Creation rejects duplicate active requests and an item already on loan to the requesting member. These implementation decisions must be included in the requirements notes and reviewed during acceptance testing.
- Automatic allocation and pickup notification after return/repair/transfer, plus patron self-service placement, remain to be implemented and integrated.
- Actual hours for this work must be supplied by the member; none have been invented here.

## 14 October reported testing and documentation update

- At 17:19, Kamalpreet supplied the generated `LinkHoldsToPickupBranches` migration. Its pickup-branch index and restricted foreign key were reviewed; no unrelated changes appear in the supplied `Up` or `Down` methods.
- At 17:31, a borrower-registration screenshot showed a valid-looking `MBR-9001` with a format error. Kamalpreet confirmed that lowercase input was visually uppercased; retyping uppercase letters allowed registration. This is genuine integration-stage usability feedback from the lead, rather than an external patron study.
- At 17:45, Kamalpreet reported completing the three guided hold tests: duplicate rejection, waiting order and waiting cancellation. Individual outcomes and the actual images have not yet been checked.
- At 17:50, Kamalpreet confirmed applying the model regex change that accepts uppercase, lowercase and mixed-case prefixes while the controller continues uppercase storage. The specific lowercase registration retest is not yet restated.
- At 17:51, Kamalpreet confirmed having three current test screenshots on the laptop with self-chosen filenames or captions. They have not been copied into the project. Old notification observations remain recorded separately from missing image assets.
- Kamalpreet records test outcomes and supplies screenshots. The evidence tables identify the inputs, result, revision and available capture. Time estimates and missing meeting details are recorded separately. [AI acknowledgement](AI_ACKNOWLEDGEMENT.md).
- The latest remote check showed `feature/holds-notifications` still at `74668e1`. The newer desk, UI, migration and documentation batch awaits a verified pushed revision.

See [hold test record](HOLD_WORKFLOW_TEST_RECORD.md), [UX feedback record](UX_FEEDBACK_RECORD.md) and [screenshot guidance](screenshots/README.md).

### Windows build/database update report — 14 October evening

Kamalpreet reported the applied circulation patch build completed at 19:55 Darwin. The console transcript supplied at 20:02 shows Build succeeded and the latest database update completed with Done / already up to date. It includes earlier branch, transfer and pickup FK updates; the new LinkPatronIdentity application is not visible. Migration-list verification and all C01–C19 workflow outcomes remain pending. These are reported/received times, not invented working hours.

At 20:06 Darwin, Kamalpreet supplied Get-Migration output showing Build succeeded and Applied=True for all seven migrations, including LinkPatronIdentity and LinkHoldsToPickupBranches. The migration-list verification is complete. Next: launch and run C01–C06 against the existing Noah/Asha queue. No workflow result or working hours are inferred from this setup check.

At 20:30 Darwin, Kamalpreet confirmed the guided pickup-notification count stayed at two after rechecking. The C03 repeat-count result is recorded as reported pass, with detailed pickup-log screenshot verification pending. Next are the wrong-member rejection, rightful collection and repeated-checkout checks. No actual working hours are inferred.

At 20:40 Darwin, Kamalpreet reported all three reservation/collection checks matched expectations: Asha was blocked, Noah borrowed and his hold displayed Collected, and a repeated checkout was blocked with one active loan. C04–C06 are recorded as reported passes pending screenshot verification. Next is a separate damaged-copy repair and Palmerston pickup journey. Actual working hours remain unreported.

At 21:17 Darwin, Kamalpreet reported the repair-allocation check produced Asha Allocated and zero pickup records for TY-9001. The allocated screenshot is held locally; the filtered zero-result image is recommended for the subsequent transfer comparison. No transfer/receipt result or working hours are inferred.

At 21:21 Darwin, the lead reported saving the recommended zero pickup-result evidence locally. Image inspection/linking remains pending until it is supplied or pushed. The transfer checks have not yet been run.

In the subsequent evening update, Kamalpreet reported all results in the latest transfer batch matched expectations: dispatch to Casuarina was rejected, dispatch to Palmerston succeeded with In transit status, reception checkout during transit was rejected, and receipt while at Darwin was rejected without changing the transit state. These results are recorded as reported passes in the circulation test record. Destination receipt and the kiosk portion of the transit check remain pending. Two rejection screenshots are recommended for this batch; their filenames/images have not been supplied. Actual hours are still not inferred from message times.

At 21:49 Darwin, Kamalpreet reported the guided Palmerston receipt step matched expectations: Asha Ready for pickup and two pickup simulations. No screenshot was taken. First receipt/displayed outcomes are recorded as reported passes, with current-screen captures and the Palmerston manual recheck requested. Repeated receipt, next-family collection/return and kiosk/API tests remain pending. This report supplies no actual working hours.

At 22:27 Darwin, Kamalpreet reported local captures of Asha Collected and Noah Ready following the guided collection/return sequence. Those displayed outcomes are reported passes pending image inspection. Six mixed-type notification results were described: two Noah rows and four Asha rows including both Loan confirmed and pickup messages. The pickup-only filter/recheck remains pending and is not treated as six duplicate pickup records. Testing guidance was clarified after source review of the filters and unfiltered CheckNow redirect. Actual hours and the prior Noah rejection outcome were not restated.

At 22:33 Darwin, the lead reported four results after the clarified pickup-only filter/recheck instructions, matching the expected pickup total for TY-9001. The next planned check uses a genuinely new Noah loan and Asha waiting request before returning the toy with NeedsRepair selected. No outcome or time spent on that next check has been inferred.

At 22:45 Darwin, Kamalpreet reported the NeedsRepair-return batch matched expectations: Noah's new checkout succeeded, Asha joined the waiting list again, the return left the copy Damaged and her new request Waiting, and the filtered pickup total stayed four. One local screenshot shows Waiting only; no other visual claims are made from it. Next planned work is repair at the already-selected pickup branch and a pickup-history restart check. Neither those future outcomes nor actual hours are inferred.

At 22:50 Darwin, the lead questioned repeated-looking test instructions. The approach was refined to explain the distinct conditions being checked and remove an extra reserved-member checkout attempt already covered by the earlier C04 report. The next batch now contains only same-branch repair allocation and pickup-history persistence/restart. The normal-return, damaged-return and cross-branch scenarios remain separate evidence; omitted or unreported historical checks are not marked passed. No additional working hours are inferred.

At 23:02 Darwin, the lead reported Waiting and four pickup records after returning TY-9001. That is consistent with the NeedsRepair-return state. A completed/saved Available edit has not been confirmed; the repair continuation remains pending. Guidance was clarified to record the completed repair via Manage toys and verify the saved status before expecting Ready and six pickup records. No defect or actual working hours are inferred.

At 23:07 Darwin, the lead reported six pickup records following the clarified completed-repair edit. The count increase is recorded as reported matching the repair expectation. The final check in this test batch is to stop/start the app and verify the same pickup-only total after startup. No restart result, exact Ready-card inspection or actual working hours is inferred from the six-record reply.

At 23:15 Darwin, the lead reported six after the guided restart, compared with six before it. Pickup-history/count stability is recorded as a reported pass. The current hold/repair/transfer batch is closed for documentation packaging; the remaining module/edge-case checks and genuine member inputs remain outstanding. Message timestamps are evidence-received times, not actual working hours.


## Screenshot audit and organisation

Kamalpreet reported the checkpoint pushed and requested review of all images and confusing filenames. Remote a1e8190 was verified; its application source matches the prepared, user-built circulation release. Twenty-one pushed images were reviewed, renamed without changing bytes and indexed with original-name mappings. Three earlier uploaded notification photos were recovered. Result captions distinguish visible state from tester reports; two names that implied more than their images show were corrected. Hours and individual contributions are still supplied by the members rather than inferred from these operations.

## Whole-commit review following the screenshot audit

The review covered all 71 changed paths in a1e8190, including 40 application source files. All 162 source/configuration assets match the prepared release Kamalpreet built. Git diff/conflict-marker checks and kiosk JavaScript syntax passed; the Windows build/runtime results are recorded separately from source inspection. Required patron placement, missing importer/report handoffs, API proposal gaps, kiosk privacy runtime checks and the notification filter-reset issue are recorded in [PROJECT_PROGRESS_REVIEW.md](PROJECT_PROGRESS_REVIEW.md), with all rubric statuses and owner deliverables. No application source was changed by this documentation package, and no hours, meetings or member experience were inferred.

## 15 October member-evidence preparation

The lead asked for assistance with hours, minutes and reflection while the documentation push completed. Remote 107f643 was verified: all 235 tracked files match the prepared review/evidence package and all 24 indexed images are present. Known tasks/evidence are now entered into the time table; no numeric hours are inferred. Two supplied written messages are recorded as coordination notes; actual phone-call details remain an intake. Kamalpreet's reflection describes her implementation and testing work. Each other member supplies their own task durations, strength/limitation and value example. These named entries will be assembled into the group portfolio; the brief does not specify four separate reflection uploads.


### Team agreement and member records

All four members accepted the original agreement in the group chat on the day it was created and shared; the calendar date is not recorded. Kamalpreet is responsible for the meeting minutes. Each member supplies their task hours and their own strength, difficulty, learning and value contribution. The group portfolio will contain four named individual sections.

## 15 October retrospective session calculation

At 09:47 Darwin the lead requested calculation from the start of the project chat, excluding gaps longer than two hours. The [time reconstruction](KAMALPREET_TIME_RECONSTRUCTION.md) records a partial recovered subtotal of **19.75 hours / approximately 19 hours 45 minutes**, including activity from 3, 4, 11, 14 and 15 October. The calculation uses original user timestamps where available, minute-level dated reports and three explicitly identified attached project-photo checkpoints. It is not a complete original message export or an exact active-time measurement. No task-level duration, other member's hours, meeting detail or personal experience has been invented. Earlier duration-pending entries describe earlier evidence states or the still-unrecorded task split.

## Patron/member push verified — 15 October

After the lead's 10:06 completion report, remote 6a69ade was fetched and compared with the combined prepared patron/evidence/member records. Every prepared file matches except the expected project dependency change and the two additional PH01/PH02 images; the package-reference set matches the fixed source and Pkcs 9.0.21 is verified. All 28 images are present. PH01/PH02 were inspected and their narrow captions added; Kamalpreet's individual reflection now includes the completed patron placement and build/package work. At that checkpoint, the remaining Sandesh branch was not included by the limited fetch. His source/report/reflection and Harsimran's guide have since been received. Their remaining corrections, Mehakpreet/Harsimran final records, real minutes, VM verification and the portfolio remain open.

## Sandesh member record received

Sandesh Bhattarai **S390535** supplied an individual testing report for **2ab17fa / PR #7**. Its local outcomes and eight original captures have been checked against the supplied report. The 12.5-hour subtotal remains a retrospective estimate, with the final testing/documentation duration unconfirmed. His first-person contribution, strength, difficulty and learning account is preserved in [SANDESH_MEMBER_HANDOFF.md](SANDESH_MEMBER_HANDOFF.md). It is evidence for his component, with the stated local-test limits.
