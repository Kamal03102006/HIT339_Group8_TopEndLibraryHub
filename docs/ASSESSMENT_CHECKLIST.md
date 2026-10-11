# HIT339 Group 8 assessment checklist

Updated: 11 October 2026. Internal completion target: 15 October 2026. Submission: 16 October 2026, 23:59 Darwin time (UTC+09:30).

This checklist tracks the supplied two-page **Group Project Management and Horizontal Feature Expansion** brief. It is a working record, not a declaration that the assessment is complete. Feature evidence currently relates to `feature/holds-notifications`, through commit `bcf6844`; it has not been verified on `master`.

## Reading the status

- **Draft**: an artifact exists but needs team review and completion.
- **Partial**: some implementation or evidence exists; acceptance checks remain.
- **Pending**: no completed artifact or result has been supplied.
- **Unverified**: another member may have work locally, but its completion has not been confirmed here.

The copied rubric has eleven ranges totalling **60 points: 37 group and 23 individual**. Criterion titles and performance descriptors were absent. Do not assign those points to the requirements below or claim an HD criterion has been met until the complete rubric is available.

## Part A: governance and documentation

| Brief requirement | Current evidence and status | Completion check | Proposed coordinator |
| --- | --- | --- | --- |
| Installation and running instructions for each physical library branch | **Partial.** Existing README covers baseline local execution. Group installation manual and physical branch selection are pending. | A new user can download the final Git version, install prerequisites, configure the database, choose each location and run the same application. Record prerequisites and troubleshooting from the actual installation. | Harsimran; all members verify |
| Test those instructions on a virtual machine | **Pending.** No VM installation evidence supplied. | Follow the manual on a clean VM; retain VM/software details, steps, results, screenshots and any corrections to the manual. | Harsimran |
| Patron user guide and/or video covering search capabilities | **Pending.** Application screenshots are not a user guide. | Explain catalogue search, filters, availability and useful patron journeys using final screens. Check the instructions with a real test user. | Mehakpreet |
| Team contracts | **Draft structure** in [GROUP_WORK_RECORD.md](GROUP_WORK_RECORD.md); acceptance and availability are pending. | Every member records their agreed responsibilities, communication arrangements, review obligations and acceptance. | Kamalpreet; each member responds |
| Requirements document and notes | **Draft.** [Requirements draft](HIT339_Group8_Requirements_Draft.docx) exists, with seven modules, acceptance checks and open questions. | Record answers, sources, dates, agreed decisions and changes; map final requirements to implementation and tests. | Kamalpreet; all module owners |
| Team approach to completing the project | **Draft proposal** in the group work record. | Team agrees the development, review, integration, testing and documentation approach and records any changes. | Kamalpreet |
| Website management links and marker read access | **Partial.** Repository and Projects board exist; marker access has not been tested. | Include the final links. Verify read access without the owner's session; supply read access details if required. If any trial service is used, it must remain available for two weeks after submission. | Kamalpreet |
| Manageable billable tasks, hours and allocations | **Draft breakdown.** Task ownership is proposed; actual group hours are not recorded. | Each task has an agreed assignee, estimated person-hours, actual person-hours and evidence. Each member records their own real time. | All members |
| Meeting minutes | **Pending.** No completed minutes supplied. | Record actual meeting dates, attendees, decisions, actions, owners and deadlines; obtain corrections from attendees. Do not reconstruct imaginary meetings. | Rotate recorder |
| Team strengths and weaknesses | **Pending.** Prompts are provided in the group work record. | Members describe their actual capabilities and limitations, with practical responses and examples. | All members |

For submission on 16 October, a trial service would need availability through at least **30 October 2026**. This condition applies only if such a service is used.

## Part B: seven horizontal expansions

Owners below reflect the proposed allocation, not confirmed acceptance by each teammate. No pushed work from another member has been verified in this audit.

| Module required by the brief | Current evidence and status | Remaining acceptance evidence | Proposed owner |
| --- | --- | --- | --- |
| Public touch-friendly kiosk: scan library code to check out or view account summary | **Unverified.** No working group kiosk demonstrated. | Show the dedicated simplified route, account summary, checkout, eligibility errors and session privacy. Test alongside reception checkout for the same item. | Mehakpreet |
| Secure read-only REST API: available catalogue items, categories and operating status | **Unverified.** No working group API demonstrated. | Show authenticated GET results for all three areas, rejected unauthorized requests, absence of patron personal data and absence of write operations. Document runnable requests. | Mehakpreet |
| Automatic simulated email or SMS: borrowing, approaching due date and accruing fines; logged messages | **Partial.** Worker, log, staff dashboard and migration are pushed. Borrowed/due-soon Email and SMS messages and a repeated check were observed. | Finish fine calculation, persistence/restart, returned-loan and access checks. Capture evidence of the scheduled trigger. Clearly label simulations. | Kamalpreet |
| Multi-branch items and reception desks, with inter-branch transfers | **Unverified.** Branch/desk/transfer implementation not supplied. | Assign items/desks to locations; dispatch and receive a transfer; prevent checkout during transit and invalid repeat transitions; provide repeat-safe multi-branch seed data. | Harsimran |
| Holds on borrowed and damaged items, reservation queue and automatic next-patron notification | **Partial foundation.** Hold model, queue service, request form and database constraints are pushed. The usable workflow is pending. | Create holds against both statuses; demonstrate FIFO and duplicate handling; allocate after return/repair; handle pickup location, cancellation and transfers; notify the next eligible patron. | Kamalpreet with Harsimran |
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
| README instructions to seed multi-branch data | **Pending.** Existing seed is the A2 baseline. | Explain how to populate and verify the agreed locations, desks, item types and demonstration scenarios. |
| README instructions to test integrations | **Partial.** Notification evidence and pending checks are recorded. | Provide reproducible inputs, expected outcomes and actual results for cross-module journeys, including invalid and repeated actions. |

## Distinctive service journey to implement and document

**A damaged toy becomes usable again, reaches the correct pickup branch and is fairly offered to the first waiting family.**

Trace one fictional item and two fictional borrowers through: damaged item → two holds → repair → first hold allocated → transfer if needed → arrival and pickup notice → kiosk checkout → return and next eligible hold. Document what each patron and staff member sees, and verify that another borrower cannot take an allocated or in-transit item.

This is an agreed-design candidate from the requirements draft; it has not yet been demonstrated. A progress view is optional and should follow completion of the required modules.

## Evidence needed from each member

For each allocated module, retain the commit/PR, requirement IDs, reproducible test steps, actual results, screenshots or response samples, real time spent and an explanation of decisions. Ask a person to attempt a real task; record their feedback, the resulting change and a retest. The lecturer's client-value and UX suggestions should be supported by actual work, not fabricated testimonials.

Individual rubric criteria remain unidentified, so this evidence is useful preparation rather than a claim about how individual marks are allocated.
