# HIT339 Group 8 assessment checklist

Current update: Sandesh source/report/reflection and eight captures are received; Harsimran's guide is received with VM setup still blocked. The original agreement was accepted on creation/sharing day. See [remaining work](REMAINING_WORK.md) for the current release and owner actions; older checkpoint findings remain historical.

Updated: 14 October 2026. Internal completion target: 15 October 2026. Submission: 16 October 2026, 23:59 Darwin time (UTC+09:30).

Source/evidence update: the combined checkpoint is verified pushed at a1e8190 on feature/holds-notifications. It contains the circulation integration, Mehakpreet's 3b925ee kiosk/API source and the earlier Harsimran branch/transfer foundation. Kamalpreet's Windows build and all seven applied migrations are confirmed. All 21 pushed images are accounted for and three historical notification photos are restored; see the screenshot index and the whole-commit review below. The final seven-module submission remains outstanding.

Automatic allocation after return/repair, shared reception/kiosk checkout, protected transfer receipt and ready-pickup simulations are implemented. Inspected images establish the main damaged queue, repair allocation without early notices, protected handover, receipt, pickup channels and collection/next-family states. The later damaged-return repair and pickup 6 → 6 restart remain tester reports. Patron-facing hold placement is implemented. Remaining gaps include importer/report corrections and integration, final kiosk/API/concurrency/privacy checks and the VM run. Genuine team records, VM verification and the final portfolio are incomplete. [The complete review](PROJECT_PROGRESS_REVIEW.md) lists the source findings, all eleven rubric rows and each member's next deliverable.

## Reading the status

- **Draft**: an artifact exists but needs team review and completion.
- **Partial**: some implementation or evidence exists; acceptance checks remain.
- **Pending**: no completed artifact or result has been supplied.
- **Unverified**: another member may have work locally, but its completion has not been confirmed here.

The two Blackboard screenshots supplied on 14 October show all eleven criterion titles and their maximum marks. Matching their order and marks to the earlier pasted group/individual labels gives **60 points: 37 group and 23 individual**. Expanded performance descriptors are not shown. The evidence below is our proposed completion plan based on the rubric headings and assignment brief, not a quotation of HD-band requirements or a predicted score.

## Confirmed rubric and evidence mapping

| Rubric criterion | Maximum | Mark type | Evidence to prepare and verify | Current position |
| --- | ---: | --- | --- | --- |
| Installation and Running instructions | 5 | Group | Exact Git download, prerequisites, database/migration/seed setup, branch/desk selection and launch steps; clean VM test details and corrections to the guide. | Baseline instructions exist; final guide and VM evidence pending. |
| Team contracts | 2 | Group | Agreed responsibilities, availability, communication, review and blocker arrangements, with each member's actual acceptance. | All four members accepted the original agreement in the group chat on its creation/sharing day; the calendar date is not recorded. Member availability remains to be added. |
| Requirements | 4 | Group | Seven-module requirements, real clarification notes and recorded decisions; traceable acceptance checks, implementation and actual test results. | Requirements draft exists; several decisions and acceptance results remain incomplete. |
| Team Approach | 4 | Group | Explain the chosen collaboration, branches, shared-model/migration coordination, review, integration and testing approach using actual examples. | Approach drafted; concrete integration/review evidence needs completing. |
| Project Plan Overview | 5 | Group | Manageable plan with milestones, dependencies, task/hours/owner summary, progress changes, management links and marker read access. | Working plan/board exist; actual progress/hours and access verification pending. |
| Project Task Allocations | 10 | Individual | For each named member, record assigned tasks, estimated and actual hours, deliverables, status, dependencies and commit/issue/test references; explain reassignment if it occurred. | Task breakdown exists; actual time and member-specific completion evidence not supplied in full. |
| Meeting Minutes | 2 | Group | Genuine dated discussions, attendees, decisions, actions, owners and deadlines, with corrections/confirmation. | Two supplied written coordination notes are recorded. Actual phone-call details are still needed before completed call minutes can be written. |
| Strengths and Weaknesses | 8 | Individual | Each member supplies their own reflection with concrete examples, a real limitation, action taken and outcome/learning need. The coordinator can format their account. | Kamalpreet's reflection is now in GROUP_WORK_RECORD.md for review; the other three personal accounts remain pending. |
| Software meets requirements | 10 | Group | Demonstrate all seven required modules in the final integrated revision; map requirements to actual runtime tests, screenshots/API responses and cross-module journeys. | Several modules have source or partial demonstrations; integrated acceptance evidence incomplete. |
| Quality of Source Code | 5 | Group | Buildable final code; readable implementation; correct validation/authorisation; consistent shared availability rules; coherent migrations; appropriate failure/repeat/concurrency checks. | Shared transactions, availability queries and identity links are implemented; Windows build and all migrations are confirmed. Remaining runtime/concurrency and final combined-source checks are pending. |
| Value Adding | 5 | Individual | Identify each member's implemented contribution, who benefits, the problem addressed, design reasoning and actual validation/feedback. Distinguish proposals from delivered improvements. | The main repair-to-pickup and next-family journey is reported tested; individual contribution/feedback evidence still needs review. A real capitalisation defect was reported, its cause inspected and the validation change applied by Kamalpreet; the specific retest outcome remains to be confirmed. See the UX feedback record. |

The first eight documentation/planning criteria total **40/60**. The three individual criteria total **23/60**: task allocations 10, strengths/weaknesses 8 and value adding 5. The leader may assemble the portfolio, but every member must supply their own real task, reflection and contribution evidence.

The patron search guide, integration diagrams, user journey maps and README requirements remain mandatory under the brief even though they are not separately named rubric rows. Include them in the final submission and connect them to the relevant evidence.

## Part A: governance and documentation

| Brief requirement | Current evidence and status | Completion check | Proposed coordinator |
| --- | --- | --- | --- |
| Installation and running instructions for each physical library branch | **Partial.** Existing README covers baseline local execution; branch/desk selection now exists in Harsimran's source. No completed guide for the final integrated application has been supplied. | A new user can download the final Git version, install prerequisites, configure the database, choose each location and run the same application. Record prerequisites and troubleshooting from the actual installation. | Harsimran; all members verify |
| Test those instructions on a virtual machine | **Pending.** No VM installation evidence supplied. | Follow the manual on a clean VM; retain VM/software details, steps, results, screenshots and any corrections to the manual. | Harsimran |
| Patron user guide and/or video covering search capabilities | **Draft.** `Documentation/PatronUserGuide.md` is included in the combined checkpoint. It needs to match final routes, screens and completed patron functionality. | Explain catalogue search, filters, availability and useful patron journeys using final screens. Check the instructions with a real test user. | Mehakpreet |
| Team contracts | All four members accepted the original agreement in the group chat, as recorded in [GROUP_WORK_RECORD.md](GROUP_WORK_RECORD.md). The original date is not recorded; availability details remain pending. | Add the original date if recovered, member availability and any agreed changes. | Kamalpreet; members supply remaining details |
| Requirements document and notes | **Draft.** [Requirements draft](HIT339_Group8_Requirements_Draft.docx) exists, with seven modules, acceptance checks and open questions. | Record answers, sources, dates, agreed decisions and changes; map final requirements to implementation and tests. | Kamalpreet; all module owners |
| Team approach to completing the project | **Draft proposal** in the group work record. | Team agrees the development, review, integration, testing and documentation approach and records any changes. | Kamalpreet |
| Website management links and marker read access | **Partial.** Repository and Projects board exist; marker access has not been tested. | Include the final links. Verify read access without the owner's session; supply read access details if required. If any trial service is used, it must remain available for two weeks after submission. | Kamalpreet |
| Manageable billable tasks, hours and allocations | **Draft breakdown.** Task ownership is proposed; actual group hours are not recorded. | Each task has an agreed assignee, estimated person-hours, actual person-hours and evidence. Each member records their own real time. | All members |
| Meeting minutes | **Pending.** No completed minutes supplied. | Record actual meeting dates, attendees, decisions, actions, owners and deadlines; obtain corrections from attendees. Do not reconstruct imaginary meetings. | Kamalpreet |
| Team strengths and weaknesses | **Draft/partial.** Kamalpreet's reflection is included in the group work record; Sandesh's account is now received; the remaining two need member input. | Members describe their actual capabilities and limitations, with practical responses and examples. | All members |

For submission on 16 October, a trial service would need availability through at least **30 October 2026**. This condition applies only if such a service is used.

## Part B: seven horizontal expansions

Owners below reflect the working allocation. Mehakpreet and Harsimran have pushed source for their modules; this is not proof that the full team contract or all acceptance checks are complete. The earlier limited fetch did not include Sandesh's branch. A full-heads fetch now confirms 2ab17fa and his report has been received; corrections and integration remain.

| Module required by the brief | Current evidence and status | Remaining acceptance evidence | Proposed owner |
| --- | --- | --- | --- |
| Public touch-friendly kiosk: scan library code to check out or view account summary | **Integrated source; build confirmed.** Mehak contribution plus shared checkout, private account/hold progress and two-minute privacy reset. PH01 account/request and PH03 inactive/unlinked images are inspected; private-data and Finish/Back/idle outcomes are reported. Final checkout/security coverage remains pending. | Demonstrate the kiosk's required checkout **or** account-summary capability; the group draft proposes both. Verify eligibility errors and session privacy. Test alongside reception checkout for the same item. | Mehakpreet |
| Secure read-only REST API: available catalogue items, categories and operating status | **Partial.** Mehakpreet's branch has all three GET areas plus configured API-key middleware. Authentication responses and final loan/hold/transfer availability have not been demonstrated together. | Show authenticated GET results for all three areas, rejected unauthorized requests, absence of patron personal data and absence of write operations. Document runnable requests and external key configuration. | Mehakpreet |
| Automatic simulated email or SMS: borrowing, approaching due date and accruing fines; logged messages | **Partial.** Worker, log, staff dashboard and migration are pushed. Borrowed/due-soon Email and SMS messages and a repeated check were observed. Pickup handover and 6 → 6 restart count are reported. | Finish fine calculation, persistence/restart, returned-loan and access checks. Capture evidence of the scheduled trigger. Clearly label simulations. | Kamalpreet |
| Multi-branch items and reception desks, with inter-branch transfers | **Partial runtime evidence.** Build/migrations confirmed; wrong destination/origin receipt, transfer receipt and first pickup handover have inspected screenshots. Transit checkout is reported. | Complete repeat receipt, setup/seed and VM verification. | Harsimran |
| Holds on borrowed and damaged items, reservation queue and automatic next-patron notification | **Staff journey implemented with observed/reported results.** FIFO/cancellation, damaged queue, repair allocation, protected receipt and next-family cards have inspected images. Repair/restart totals remain reported. **Patron placement is now implemented** at /Kiosk/Hold, with inspected PH01 request/PH02 shared FIFO positions and PH03 membership-blocking images. Duplicate/private-data and Finish/Back/idle actions remain reported. | Complete the focused forged-input/antiforgery and JavaScript-disabled stale-request checks, shared kiosk checkout and concurrency. Preserve the completed PH evidence rather than repeating the same captures. | Kamalpreet with Mehakpreet/Harsimran |
| Administrative bulk import from CSV or a mock metadata provider | **Unverified.** CSV import is proposed; no demonstration supplied. | Import valid records, explain row errors, reject duplicates safely, preserve existing data and show administrative access control. Include sample files. | Sandesh |
| Expanded manager reports, with CSV/PDF exports of borrowing, fine revenue audit and inventory health | **Partial baseline only.** A2 manager statistics work; the required group expansion and exports are unverified. | Reconcile all three report areas with known data; distinguish money owed from money collected; apply selected filters consistently to screen/export; test permissions. | Sandesh |

The brief writes “CSV/PDF exports”; the requirements draft proposes both. Confirm the agreed interpretation and document it rather than silently reducing scope. Implement the required breadth before optional additions or extensive refactoring.

## Final deliverables

| Deliverable | Current status | Completion check |
| --- | --- | --- |
| Group Portfolio PDF containing all project management artifacts | **Pending assembly.** Requirements and these working records are inputs, not the final portfolio. | Include every Part A artifact, readable evidence, final requirement/test references and consistent group details. Export and inspect the complete PDF. |
| Integration diagrams for the new breadth features | **Draft source-matched diagram** in ARCHITECTURE_AND_JOURNEYS.md; final combined application and PDF verification pending. | Explain how kiosk/API/notifications/branches/holds/import/reports connect to the shared MVC application and data. Match the final implementation. |
| User journey maps for the new features | **Draft journeys** in ARCHITECTURE_AND_JOURNEYS.md; repair/transfer/kiosk runtime results and final portfolio pending. | Show patron and staff goals, actions, decisions and feedback, including the repair-to-pickup journey. Validate against the final application. |
| Expanded .NET 10 MVC source repository link | **Partial.** Public repository exists; final integrated version is pending. | Deliver a buildable integrated revision with migrations and repeat-safe seeds. Identify the final branch, commit or tag. |
| README navigation to the new modules | **Updated working README.** Actual/prepared routes, branch/patron seeds and reproducible integration inputs are documented; final acceptance/VM verification pending. | Add final routes, roles and demonstration steps for all seven modules; do not list planned screens as working. |
| README instructions to seed multi-branch data | **Updated working README.** Actual/prepared routes, branch/patron seeds and reproducible integration inputs are documented; final acceptance/VM verification pending. | Explain how to populate and verify the agreed locations, desks, item types and demonstration scenarios. |
| README instructions to test integrations | **Updated working README.** Actual/prepared routes, branch/patron seeds and reproducible integration inputs are documented; final acceptance/VM verification pending. | Provide reproducible inputs, expected outcomes and actual results for cross-module journeys, including invalid and repeated actions. |

## Distinctive service journey to implement and document

**A damaged toy becomes usable again, reaches the correct pickup branch and is fairly offered to the first waiting family.**

Trace one fictional item and two fictional borrowers through: damaged item → two holds → repair → first hold allocated → transfer if needed → arrival and pickup notice → kiosk checkout → return and next eligible hold. Document what each patron and staff member sees, and verify that another borrower cannot take an allocated or in-transit item.

This is a design candidate from the requirements draft, with reported repair/allocation/dispatch results now recorded. Destination arrival, staff collection, next-family readiness, a damaged return and later repair now have reported outcomes. Kiosk collection and final image matching remain separate; the whole seven-module application is not marked passed. A progress view is optional and should follow completion of the required modules.

## Evidence needed from each member

For each allocated module, members supply the commit/PR, reproducible inputs, actual results, available screenshots or responses, real time spent and decisions. The written records are assembled from those inputs, with requirement links added during documentation review. Ask a person to attempt a real task; record their feedback, the resulting change and a retest. The lecturer's client-value and UX suggestions should be supported by actual work, not fabricated testimonials.

Use the confirmed individual rows above when collecting member evidence. A task allocation table alone is not the complete individual record: include actual delivery evidence, personal reflection and the member's demonstrated value contribution. The rubric screenshots do not establish a particular score for any artifact.

## Next completion priorities, as of 14 October

1. Use the confirmed eleven rubric rows and the full brief as the submission checklist. If detailed marking descriptors become available, refine the evidence mapping without delaying authorised work.
2. Package the completed current circulation batch, then complete kiosk/API, concurrency, repeat receipt and remaining loan/fine notification acceptance checks.
3. Obtain and review Sandesh's importer/report handoff, integrate it, then verify and identify the final pushed source revision.
4. Collect each member's actual hours, contract acceptance, strengths/weaknesses, real meeting records and test/UX evidence.
5. Complete and test the installation manual on a VM; update the patron guide and README against the final application.
6. Assemble the portfolio PDF with all Part A artifacts, integration diagrams and user journey maps; inspect the exported PDF and verify marker access and final repository revision.

Do not treat prepared views, draft records, a source commit or an author's test claim as equivalent to verified final submission evidence. Missing local evidence is not a claim that a teammate has done no work.

## Evidence update on 14 October

Documentation is maintained from reported tests and inspected source/images; the lead does not need to write the result tables. All 21 pushed images are now indexed and three original historical photos are restored. Earlier notes that images were local or unavailable describe past checkpoints. Collect any further evidence during meaningful remaining checks, rather than recreating every old step.


## Screenshot audit checkpoint

Twenty-one pushed images have been matched and captioned; three historical uploaded notification photos are restored. The [screenshot index](screenshots/README.md) and [audit](SCREENSHOT_AUDIT.md) identify coverage and gaps. Additional image observations establish the damaged queue, allocation/no early notice, dispatch/receipt errors and successful handover/collection. Input-only captures and the later reported repair/restart counts remain labelled accurately. This is not completion of kiosk/API, import/reports, VM verification or the portfolio.

## Member records update — 15 October

Known tasks/dates/evidence are entered under Actual time record in GROUP_WORK_RECORD.md. Kamalpreet's member-selected two-hour session rule has a [partial reconstructed subtotal of 19.75 hours](KAMALPREET_TIME_RECONSTRUCTION.md); this is not a complete active-time timesheet. Individual task splits, unretrieved/offline sessions and Mehakpreet/Harsimran durations remain unrecorded; Sandesh has supplied a separate partial retrospective estimate. Any further untracked duration must be labelled as a retrospective estimate with its basis. The same file contains the team agreement, written coordination notes, a meeting-minutes intake owned by Kamalpreet and her individual reflection. These records are source material for the final portfolio, not a claim that all hours, minutes or individual entries are complete.

## Verified patron/member push — 15 October

Checkpoint **6a69ade** includes patron update **70db5b7**, Pkcs 9.0.21 and the member-record draft. The pushed source/package set matches the prepared release, and all **28** images are present. New PH01/PH02 captures are inspected and indexed; PH03 keeps its existing limits. Patron placement is no longer an unimplemented source gap. Remaining acceptance, importer/report corrections and integration, a successful clean-VM run, missing team/member records and the final PDF remain required. Kamalpreet's reflection includes today's work; Sandesh's reflection is now received; Mehakpreet and Harsimran still supply their individual accounts.
