# HIT339 Group 8 assessment checklist

Updated: 14 October 2026. Internal completion target: 15 October 2026. Submission: 16 October 2026, 23:59 Darwin time (UTC+09:30).

Source/evidence update: feature/holds-notifications remains verified at dbbc540 with five committed screenshots covering three observed hold UI passes. Harsimran's branches/transfers are in that baseline. Kamalpreet applied the circulation patch incorporating Mehakpreet's 3b925ee source; Windows build and all seven applied migrations are confirmed. This applied integration batch has not yet been verified as a new pushed revision.

Automatic allocation after return/repair, shared reception/kiosk checkout, reservation-preserving transfer receipt and ready-pickup simulations are implemented. New screenshots show Noah Ready after the guided return and borrowing rejection in the damaged-copy scenario. User reports confirm repeat pickup count stability, reserved-member checkout rules, repair allocation without an early pickup notice, correct dispatch and wrong-destination/transit-checkout/origin-receipt guards. First Palmerston receipt, Asha Collected, Noah Ready, damaged-return waiting and the later repair are reported. Pickup history stayed 6 → 6 after restart. Detailed images, repeated receipt, kiosk/API, concurrency and loan recovery, import/report handoff, VM run and the final portfolio remain outstanding. See the circulation record for the scope of each reported or observed result.

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
| Team contracts | 2 | Group | Agreed responsibilities, availability, communication, review and blocker arrangements, with each member's actual acceptance. | Draft terms and proposed allocation exist; acceptance/availability records incomplete. |
| Requirements | 4 | Group | Seven-module requirements, real clarification notes and recorded decisions; traceable acceptance checks, implementation and actual test results. | Requirements draft exists; several decisions and acceptance results remain incomplete. |
| Team Approach | 4 | Group | Explain the chosen collaboration, branches, shared-model/migration coordination, review, integration and testing approach using actual examples. | Approach drafted; concrete integration/review evidence needs completing. |
| Project Plan Overview | 5 | Group | Manageable plan with milestones, dependencies, task/hours/owner summary, progress changes, management links and marker read access. | Working plan/board exist; actual progress/hours and access verification pending. |
| Project Task Allocations | 10 | Individual | For each named member, record assigned tasks, estimated and actual hours, deliverables, status, dependencies and commit/issue/test references; explain reassignment if it occurred. | Task breakdown exists; actual time and member-specific completion evidence not supplied in full. |
| Meeting Minutes | 2 | Group | Genuine dated discussions, attendees, decisions, actions, owners and deadlines, with corrections/confirmation. | Template exists; completed minutes pending. |
| Strengths and Weaknesses | 8 | Individual | Each member writes their own reflection with concrete examples, a real limitation, action taken and its outcome or remaining learning need. | Prompts exist; personal entries pending. |
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
| Patron user guide and/or video covering search capabilities | **Draft.** `Documentation/PatronUserGuide.md` exists on Mehakpreet's branch. It needs to match final routes, screens and completed patron functionality. | Explain catalogue search, filters, availability and useful patron journeys using final screens. Check the instructions with a real test user. | Mehakpreet |
| Team contracts | **Draft structure** in [GROUP_WORK_RECORD.md](GROUP_WORK_RECORD.md); acceptance and availability are pending. | Every member records their agreed responsibilities, communication arrangements, review obligations and acceptance. | Kamalpreet; each member responds |
| Requirements document and notes | **Draft.** [Requirements draft](HIT339_Group8_Requirements_Draft.docx) exists, with seven modules, acceptance checks and open questions. | Record answers, sources, dates, agreed decisions and changes; map final requirements to implementation and tests. | Kamalpreet; all module owners |
| Team approach to completing the project | **Draft proposal** in the group work record. | Team agrees the development, review, integration, testing and documentation approach and records any changes. | Kamalpreet |
| Website management links and marker read access | **Partial.** Repository and Projects board exist; marker access has not been tested. | Include the final links. Verify read access without the owner's session; supply read access details if required. If any trial service is used, it must remain available for two weeks after submission. | Kamalpreet |
| Manageable billable tasks, hours and allocations | **Draft breakdown.** Task ownership is proposed; actual group hours are not recorded. | Each task has an agreed assignee, estimated person-hours, actual person-hours and evidence. Each member records their own real time. | All members |
| Meeting minutes | **Pending.** No completed minutes supplied. | Record actual meeting dates, attendees, decisions, actions, owners and deadlines; obtain corrections from attendees. Do not reconstruct imaginary meetings. | Rotate recorder |
| Team strengths and weaknesses | **Pending.** Prompts are provided in the group work record. | Members describe their actual capabilities and limitations, with practical responses and examples. | All members |

For submission on 16 October, a trial service would need availability through at least **30 October 2026**. This condition applies only if such a service is used.

## Part B: seven horizontal expansions

Owners below reflect the working allocation. Mehakpreet and Harsimran have pushed source for their modules; this is not proof that the full team contract or all acceptance checks are complete. No Sandesh feature branch was visible in the fetched references on 14 October; he may have unpushed work.

| Module required by the brief | Current evidence and status | Remaining acceptance evidence | Proposed owner |
| --- | --- | --- | --- |
| Public touch-friendly kiosk: scan library code to check out or view account summary | **Integrated source; build confirmed.** Mehak contribution plus shared checkout, private account/hold progress and two-minute privacy reset. Kiosk runtime verification is pending. | Demonstrate the kiosk's required checkout **or** account-summary capability; the group draft proposes both. Verify eligibility errors and session privacy. Test alongside reception checkout for the same item. | Mehakpreet |
| Secure read-only REST API: available catalogue items, categories and operating status | **Partial.** Mehakpreet's branch has all three GET areas plus configured API-key middleware. Authentication responses and final loan/hold/transfer availability have not been demonstrated together. | Show authenticated GET results for all three areas, rejected unauthorized requests, absence of patron personal data and absence of write operations. Document runnable requests and external key configuration. | Mehakpreet |
| Automatic simulated email or SMS: borrowing, approaching due date and accruing fines; logged messages | **Partial.** Worker, log, staff dashboard and migration are pushed. Borrowed/due-soon Email and SMS messages and a repeated check were observed. Pickup handover and 6 → 6 restart count are reported. | Finish fine calculation, persistence/restart, returned-loan and access checks. Capture evidence of the scheduled trigger. Clearly label simulations. | Kamalpreet |
| Multi-branch items and reception desks, with inter-branch transfers | **Partial runtime evidence.** Build/migrations confirmed. Correct dispatch, invalid-action guards and first Palmerston receipt/Asha Ready are user-reported passes. Detailed images, repeat receipt and VM checks remain pending. | Assign items/desks to locations; dispatch and receive a transfer; prevent checkout during transit and invalid repeat transitions; preserve hold allocation at receipt; provide repeat-safe multi-branch seed data. | Harsimran |
| Holds on borrowed and damaged items, reservation queue and automatic next-patron notification | **Partial, observed and reported evidence.** Five committed screenshots verify duplicate/FIFO/cancellation; a new screenshot shows Noah Ready. Wrong-member rejection, rightful collection, repeat-checkout protection and Asha's repair allocation with zero early pickup records are reported. Guided Palmerston receipt produced Asha Ready and two pickup records by report. Collection/next-family handover and damaged-return repair are now reported; pickup restart count stayed 6 → 6. Detailed messages, permissions/inactive eligibility and shared kiosk/concurrency checks remain pending. | Create holds against both statuses; demonstrate FIFO and duplicate handling; allocate after return/repair; handle pickup location, cancellation and transfers; notify the next eligible patron. | Kamalpreet with Harsimran |
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

The documentation is maintained from reported tests and inspected source. The lead does not need to write the result tables. Five initial hold screenshots are committed; later circulation captures are held locally and can be copied to `docs/screenshots` using their existing filenames. Old notification observations are retained, while missing photo assets are identified as unavailable. Additional screenshots will be collected during meaningful final acceptance checks rather than reconstructing every old step.
