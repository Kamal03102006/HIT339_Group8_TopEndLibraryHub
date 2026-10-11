# HIT339 Group 8 working governance record

Version 0.1, 11 October 2026. This contains proposed arrangements, verified progress and blank records for genuine team input. It is **not** an accepted contract, completed meeting minutes or a final portfolio.

## Team contract: proposed terms for review

Group members: Kamalpreet, Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai.

1. Confirm each member's responsibilities and available working times before treating the allocation as agreed.
2. Keep development in named Git feature branches; include meaningful commits and a short description of how to run and verify the change.
3. Agree shared database models, migrations and borrowing/hold/transfer rules before changes that affect another module. Coordinate migration generation to avoid conflicting snapshots.
4. Keep source, acceptance evidence and documentation updated together. Each member records their own real hours and explains their own design decisions.
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
| D02 / Q2 | Confirm how a patron identifies themselves at the kiosk, whether a PIN is needed, scanner input and session timeout. | Pending |
| D03 / Q3 | Confirm API consumer, authentication and what operating status must expose. Avoid patron personal data in the read-only API. | Pending |
| D04 / Q4 | Confirm loan period, $1.50 daily fine proposal, Darwin date rules and any restrictions for unpaid fines. | Pending |
| D05 / Q5 | Confirm holds are for physical copies, FIFO policy, cancellation/expiry and transfer handover rules. | Pending |
| D06 / Q6 | Confirm CSV error handling, required fields and whether both CSV and PDF report exports are required. | Pending |
| D07 / integration | Harsimran's branch model and reception desk model are needed to connect the scalar pickup branch ID and the complete hold/transfer workflow. | Model/API agreement pending |

For each changed decision, record the reason, affected requirement IDs, affected members and commit/test references. Keep the draft requirements consistent with the actual agreement.

## Billable task breakdown

Estimates below are **planning placeholders in person-hours**, to be revised and accepted by each owner. They are not hours already worked. Baseline September hours in `DEVELOPMENT_LOG.md` are Assignment 2 history and must not be counted as group-project time.

| Task | Proposed owner | Est. hours | Actual hours | Evidence / dependency |
| --- | --- | ---: | --- | --- |
| Agree requirements answers, roles and contract | All; Kamalpreet coordinates | 2 | Not recorded | D01–D07; acceptance record |
| Notification implementation and migration | Kamalpreet | 4 | Not recorded | Commits `a8a5dbe`, `1e052e7`, `26f1fbd`, `bcf6844`; implementation exists |
| Complete notification acceptance evidence | Kamalpreet | 2 | Not recorded | Notification test record; several checks pending |
| Hold foundation and queue constraints | Kamalpreet | 3 | Not recorded | Commits `683b508` through `24094c6`; foundation exists |
| Hold creation, cancellation and fair allocation | Kamalpreet | 4 | Not recorded | Requires agreed branch and item availability rules |
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

| Member | Strength with an example | Limitation or learning need | Agreed response |
| --- | --- | --- | --- |
| Kamalpreet | To enter | To enter | To enter |
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
