# HIT339 Group 8 working governance record

Version 0.3, updated 14 October 2026. This contains proposed arrangements, verified progress and blank records for genuine team input. It is **not** an accepted contract, completed meeting minutes or a final portfolio.

## Team contract: proposed terms for review

Group members: Kamalpreet, Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai.

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
| Kamalpreet | Lead coordination; requirements; holds; simulated notifications; integration evidence | Lead role stated by user. Final group allocation still to be recorded. | To enter |
| Harsimran Singh | Branches, reception desks, transfers; installation manual and VM test | To confirm | To enter |
| Mehakpreet Kaur | Kiosk and read-only API; patron guide and UX evidence | To confirm | To enter |
| Sandesh Bhattarai | CSV import and expanded reports/exports; portfolio assembly | To confirm | To enter |

## Proposed approach to completing the project

Use the selected Assignment 2 MVC application as the baseline and add the seven required horizontal modules. Confirm requirements and interfaces, implement in feature branches, demonstrate each module against its acceptance checks, then integrate and test a shared patron/staff journey. Update the technical guide, patron guide, evidence record and task log after each meaningful change. Reserve 15 October for final integrated testing, VM instructions, document checks and portfolio assembly.

The fictional family-lending focus is books, music and toys across Darwin Hub, Palmerston Hub and Casuarina Hub. These are proposed demonstration locations, not claims about real libraries. Prioritize breadth and coherent integration before optional enhancements or deep architectural refactoring.

## Requirements decisions and notes

The [requirements draft](HIT339_Group8_Requirements_Draft.docx) contains acceptance checks and open questions. Record actual answers here; a proposal is not a client or team approval.

| Decision | Current proposal / question to resolve | Agreed answer, respondent and date |
| --- | --- | --- |
| D01 / Q1 | Confirm allocation, availability and the family-lending demonstration focus. | Pending |
| D02 / Q2 | Proposed use of existing ASP.NET Core Identity with a unique optional link from each Borrower to its patron login; resolve private account access from the authenticated user ID. Scanner input and kiosk session reset/timeout need verification. | Mehakpreet requested coordination on 11 October. Her branch at `3b925ee` now includes the optional unique Identity link, its migration and an account action that resolves Borrower from the authenticated user ID. Checkout deliberately remains blocked until shared borrowing rules are integrated. Team acceptance and runtime integration checks remain to be recorded. |
| D03 / Q3 | Read-only API consumer and usage contract need confirmation. Mehakpreet has implemented configurable API-key middleware and available-items, categories and operating-status endpoints. Avoid patron personal data. | Source reviewed through `3b925ee` on 14 October. GET catalogue, available, categories and system/database status actions exist; final hold/transfer availability rules, authentication results and consumer contract remain to be verified. |
| D04 / Q4 | Confirm loan period, $1.50 daily fine proposal, Darwin date rules and any restrictions for unpaid fines. | Pending |
| D05 / Q5 | Confirm holds are for physical copies, FIFO policy, cancellation/expiry and transfer handover rules. | Pending |
| D06 / Q6 | Confirm CSV error handling, required fields and whether both CSV and PDF report exports are required. | Pending |
| D07 / integration | Connect Hold.PickupBranchId to Branch.Id. Item.BranchId records physical location; a transfer keeps the origin until destination receipt. Preserve hold allocation during and after transfer. | Harsimran supplied the branch contract on 12 October. Source verified on 14 October in `master` at `99aa527`: branches/desks, transfers and `ItemStatus.InTransit = 5` exist. Kamalpreet generated the restricted Hold pickup-branch foreign key and index on 14 October; the supplied migration was reviewed. Receipt/notification integration remains pending. Coordinate snapshot/migration order with Mehakpreet's patron-link change. |

For each changed decision, record the reason, affected requirement IDs, affected members and commit/test references. Keep the draft requirements consistent with the actual agreement.

## Billable task breakdown

Estimates below are **planning placeholders in person-hours**, to be revised and accepted by each owner. They are not hours already worked. Baseline September hours in `DEVELOPMENT_LOG.md` are Assignment 2 history and must not be counted as group-project time.

| Task | Proposed owner | Est. hours | Actual hours | Evidence / dependency |
| --- | --- | ---: | --- | --- |
| Agree requirements answers, roles and contract | All; Kamalpreet coordinates | 2 | Not recorded | D01–D07; acceptance record |
| Notification implementation and migration | Kamalpreet | 4 | Not recorded | Commits `a8a5dbe`, `1e052e7`, `26f1fbd`, `bcf6844`; implementation exists |
| Complete notification acceptance evidence | Kamalpreet | 2 | Not recorded | Notification test record; several checks pending |
| Hold foundation and queue constraints | Kamalpreet | 3 | Not recorded | Commits `683b508` through `24094c6`; foundation exists |
| Hold creation, cancellation and fair allocation | Kamalpreet | 4 | Not recorded | Creation, eligible queue display and waiting cancellation are implemented in the desk batch; three guided tests were reported completed on 14 October. Automatic allocation remains pending. |
| Hold return/repair/transfer integration and tests | Kamalpreet + Harsimran | 3 | Not recorded | Working branch/transfer model and callbacks required |
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

### Actual time entry template

| Date | Member | Task | Actual hours | Work performed | Commit or evidence |
| --- | --- | --- | --- | --- | --- |
| To enter | To enter | To enter | To enter from actual record | To enter | To enter |

If an estimate of past time is necessary, label it a retrospective estimate and explain its basis; do not present it as a contemporaneous timesheet.

## Verified progress and blockers

| Date | Work or event | Evidence | Limit / next action |
| --- | --- | --- | --- |
| 4 October | Group requirements draft added to repository | Commit `f6c75b9` | Review and answer open questions |
| Observed by 11 October | Holds foundation, notification implementation and migration pushed | Feature branch through `bcf6844` | Complete hold workflow and remaining runtime checks |
| 11 October | User sent Harsimran a request for branch/desk model progress and a pushed branch name | User reported sending the drafted message | No reply or member branch supplied in this audit; this is not evidence that he has done no work |
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

## Meeting minutes template — no meeting recorded yet

Fill this after an actual discussion; do not label the template as a held meeting.

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
| Kamalpreet | Coordinates module dependencies and reports concrete integration and UX problems; examples include pickup-branch coordination and the membership capitalisation defect. | Developing independence with unfamiliar Git/EF integration and written evidence; the need for documentation support was explicitly reported. | Uses guided build/test batches and reports actual behaviour; written records are assembled with assistance. This draft reflection requires Kamalpreet's review before the portfolio. |
| Harsimran Singh | To enter | To enter | To enter |
| Mehakpreet Kaur | To enter | To enter | To enter |
| Sandesh Bhattarai | To enter | To enter | To enter |

Use concrete examples such as a tested workflow, database review, guide written or issue resolved. Choose actions that respond to actual limitations, such as a paired review or an earlier integration checkpoint.

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

- The user reported the merge/build/commit/push batch complete at 16:38 Darwin. The pushed commit `74668e1` was verified and its three shared files match the prepared merge source. Detailed integrated runtime results are still pending.
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
- The lead reports test outcomes in conversation. Documentation is compiled from those reports with AI assistance, then reviewed by the group. This arrangement does not turn estimated hours, proposed contracts or unreported meetings into completed evidence.
- The latest remote check showed `feature/holds-notifications` still at `74668e1`. The newer desk, UI, migration and documentation batch awaits a verified pushed revision.

See [hold test record](HOLD_WORKFLOW_TEST_RECORD.md), [UX feedback record](UX_FEEDBACK_RECORD.md) and [screenshot guidance](screenshots/README.md).
