# HIT339 Group 8 assessment checklist

Updated: 14 October 2026. Internal completion target: 15 October 2026. Submission: 16 October 2026, 23:59 Darwin time (UTC+09:30).

This checklist tracks the supplied two-page **Group Project Management and Horizontal Feature Expansion** brief, re-read on 14 October. It is a working record, not a declaration that the assessment is complete. Source reviewed: `feature/holds-notifications` through `74668e1`, `feature/mehakpreet-kiosk-api` through `3b925ee`, and `master` through `99aa527` (including Harsimran's branch through `75a1a9e`). Source presence is distinct from a successful build, demonstrated runtime behaviour or final integration. The user reported the branch merge/build/commit/push batch complete on 14 October, and the pushed merge was verified at `74668e1`. Full runtime integration, including Mehakpreet's modules, remains unverified.

The green homepage/layout/stylesheet and the first usable holds desk were prepared against merge `74668e1`. Kamalpreet generated and supplied the pickup-branch migration for review, reports completing the three guided hold checks on 14 October, and confirms applying the membership validation fix. Three screenshots are held on the laptop, with individual outcomes and image references awaiting confirmation. The tested batch is not yet verified in a subsequent pushed commit. Automatic allocation, patron self-service placement and loan/repair/transfer handover remain subsequent integration work.

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
| Quality of Source Code | 5 | Group | Buildable final code; readable implementation; correct validation/authorisation; consistent shared availability rules; coherent migrations; appropriate failure/repeat/concurrency checks. | Branches/transfers and holds/notifications are combined at `74668e1`; the user reports the merge batch complete. Mehakpreet's contribution and remaining features still need integration and final review. |
| Value Adding | 5 | Individual | Identify each member's implemented contribution, who benefits, the problem addressed, design reasoning and actual validation/feedback. Distinguish proposals from delivered improvements. | Repair-to-pickup integration remains in progress. A real capitalisation defect was reported, its cause inspected and the validation change applied by Kamalpreet; the specific retest outcome remains to be confirmed. See the UX feedback record. |

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
| Public touch-friendly kiosk: scan library code to check out or view account summary | **Partial.** Mehakpreet's branch at `3b925ee` includes browsing, scanner-style lookup and a private account action using the authenticated user's unique optional Borrower link. Its migration also adds a unique active-loan index. Checkout explicitly remains blocked until the shared borrowing service is integrated; these changes are not yet merged or runtime verified. | Demonstrate the kiosk's required checkout **or** account-summary capability; the group draft proposes both. Verify eligibility errors and session privacy. Test alongside reception checkout for the same item. | Mehakpreet |
| Secure read-only REST API: available catalogue items, categories and operating status | **Partial.** Mehakpreet's branch has all three GET areas plus configured API-key middleware. Authentication responses and final loan/hold/transfer availability have not been demonstrated together. | Show authenticated GET results for all three areas, rejected unauthorized requests, absence of patron personal data and absence of write operations. Document runnable requests and external key configuration. | Mehakpreet |
| Automatic simulated email or SMS: borrowing, approaching due date and accruing fines; logged messages | **Partial.** Worker, log, staff dashboard and migration are pushed. Borrowed/due-soon Email and SMS messages and a repeated check were observed. | Finish fine calculation, persistence/restart, returned-loan and access checks. Capture evidence of the scheduled trigger. Clearly label simulations. | Kamalpreet |
| Multi-branch items and reception desks, with inter-branch transfers | **Partial, source reviewed.** Branch/desk models, item locations, seed data, staff location selection, transfer dispatch/receipt and `ItemStatus.InTransit = 5` are now in `master`. Book/Music/Toy creation and edit location handling are also updated. Transfer receipt currently restores general availability and explicitly leaves hold integration pending. No integrated runtime or VM test evidence has been supplied. | Assign items/desks to locations; dispatch and receive a transfer; prevent checkout during transit and invalid repeat transitions; preserve hold allocation at receipt; provide repeat-safe multi-branch seed data. | Harsimran |
| Holds on borrowed and damaged items, reservation queue and automatic next-patron notification | **Partial.** Foundation and constraints are pushed. Staff-assisted creation, waiting-list display/cancellation and pickup-branch relationship are in the desk batch. Kamalpreet supplied the generated migration and reports completing three guided tests; detailed outcomes, screenshot references and the pushed revision are still to be confirmed. Allocation and handover integration remain incomplete. | Create holds against both statuses; demonstrate FIFO and duplicate handling; allocate after return/repair; handle pickup location, cancellation and transfers; notify the next eligible patron. | Kamalpreet with Harsimran |
| Administrative bulk import from CSV or a mock metadata provider | **Unverified.** CSV import is proposed; no demonstration supplied. | Import valid records, explain row errors, reject duplicates safely, preserve existing data and show administrative access control. Include sample files. | Sandesh |
| Expanded manager reports, with CSV/PDF exports of borrowing, fine revenue audit and inventory health | **Partial baseline only.** A2 manager statistics work; the required group expansion and exports are unverified. | Reconcile all three report areas with known data; distinguish money owed from money collected; apply selected filters consistently to screen/export; test permissions. | Sandesh |

The brief writes “CSV/PDF exports”; the requirements draft proposes both. Confirm the agreed interpretation and document it rather than silently reducing scope. Implement the required breadth before optional additions or extensive refactoring.

## Final deliverables

| Deliverable | Current status | Completion check |
| --- | --- | --- |
| Group Portfolio PDF containing all project management artifacts | **Pending assembly.** Requirements and these working records are inputs, not the final portfolio. | Include every Part A artifact, readable evidence, final requirement/test references and consistent group details. Export and inspect the complete PDF. |
| Integration diagrams for the new breadth features | **Pending.** | Explain how kiosk/API/notifications/branches/holds/import/reports connect to the shared MVC application and data. Match the final implementation. |
| User journey maps for the new features | **Pending.** | Show patron and staff goals, actions, decisions and feedback, including the repair-to-pickup journey. Validate against the final application. |
| Expanded .NET 10 MVC source repository link | **Partial.** Public repository exists; final integrated version is pending. | Deliver a buildable integrated revision with migrations and repeat-safe seeds. Identify the final branch, commit or tag. |
| README navigation to the new modules | **Partial.** Notifications are documented; remaining routes are pending. | Add final routes, roles and demonstration steps for all seven modules; do not list planned screens as working. |
| README instructions to seed multi-branch data | **Partial source.** Harsimran's repeat-safe branch/desk seeds and migration are in `master`; final README instructions and verification are still pending. | Explain how to populate and verify the agreed locations, desks, item types and demonstration scenarios. |
| README instructions to test integrations | **Partial.** Notification evidence and pending checks are recorded. | Provide reproducible inputs, expected outcomes and actual results for cross-module journeys, including invalid and repeated actions. |

## Distinctive service journey to implement and document

**A damaged toy becomes usable again, reaches the correct pickup branch and is fairly offered to the first waiting family.**

Trace one fictional item and two fictional borrowers through: damaged item → two holds → repair → first hold allocated → transfer if needed → arrival and pickup notice → kiosk checkout → return and next eligible hold. Document what each patron and staff member sees, and verify that another borrower cannot take an allocated or in-transit item.

This is an agreed-design candidate from the requirements draft; it has not yet been demonstrated. A progress view is optional and should follow completion of the required modules.

## Evidence needed from each member

For each allocated module, members supply the commit/PR, reproducible inputs, actual results, available screenshots or responses, real time spent and decisions. The written records are assembled from those inputs, with requirement links added during documentation review. Ask a person to attempt a real task; record their feedback, the resulting change and a retest. The lecturer's client-value and UX suggestions should be supported by actual work, not fabricated testimonials.

Use the confirmed individual rows above when collecting member evidence. A task allocation table alone is not the complete individual record: include actual delivery evidence, personal reflection and the member's demonstrated value contribution. The rubric screenshots do not establish a particular score for any artifact.

## Next completion priorities, as of 14 October

1. Use the confirmed eleven rubric rows and the full brief as the submission checklist. If detailed marking descriptors become available, refine the evidence mapping without delaying authorised work.
2. Finish the green UI build check and integrate the latest source contributions with a combined model snapshot.
3. Complete holds, patron account/checkout linkage, and loan/repair/transfer notification integration; obtain Sandesh's importer/report handoff.
4. Collect each member's actual hours, contract acceptance, strengths/weaknesses, real meeting records and test/UX evidence.
5. Complete and test the installation manual on a VM; update the patron guide and README against the final application.
6. Assemble the portfolio PDF with all Part A artifacts, integration diagrams and user journey maps; inspect the exported PDF and verify marker access and final repository revision.

Do not treat prepared views, draft records, a source commit or an author's test claim as equivalent to verified final submission evidence. Missing local evidence is not a claim that a teammate has done no work.

## Evidence update on 14 October

The documentation is maintained from reported tests and inspected source. The lead does not need to write the result tables. Three current hold-test screenshots are available locally and can be copied to `docs/screenshots` using their existing filenames. Old notification observations are retained, while missing photo assets are identified as unavailable. Additional screenshots will be collected during meaningful final acceptance checks rather than reconstructing every old step.
