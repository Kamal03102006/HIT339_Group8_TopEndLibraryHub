# Our discussion minutes

HIT339 Group 8 | Recorder: Kamalpreet, S389008

We compiled these minutes from our discussions and recorded follow-ups. Dates identify the follow-ups. Exact start/end times and complete attendance for the earlier conversations were not recorded.

## M01 - Choosing the project and dividing the work

Dates: 3-4 October 2026

We chose Kamalpreet's Assignment 2 project as our shared starting point. We wanted the new modules to work together: a family could request an unavailable toy, keep its place during repair/transfer and collect at its chosen branch.

| Responsibility | Owner | Next action |
| --- | --- | --- |
| Holds and simulated messages | Kamalpreet | Define queue rules and coordinate requirements. |
| Branches/desks/transfers | Harsimran | Share the foundation before dependent modules. |
| Kiosk/API | Mehakpreet | Review login and borrower ownership before checkout. |
| Importer/manager exports | Sandesh | Review Book and report data structures. |

We each needed to clone, build and run the project and retain our hours/tests/contribution evidence. Repository access was being arranged; Sandesh accepted the invitation on 4 October.

## M02 - Schedule and dependencies

Dates: 10-12 October 2026

We set 15 October as our internal completion target, ahead of the official 16 October deadline. The first feature target was 13 October; on 11 October we requested working handoffs by 14 October so that integration/documentation could use 14-15 October.

The branch foundation was needed by holds, kiosk and imports. By 12 October it was running while transfers were still in progress. The patron login needed an explicit Borrower link.

| Action | Owner |
| --- | --- |
| Share branch/desk models, item location, migrations and seeds. | Harsimran |
| Align Identity, checkout, hold and transfer guards. | Mehakpreet and Kamalpreet |
| Define branch mapping for imports and separate unpaid fines from collected revenue. | Sandesh |

## M03 - Bringing the branches together

Date: 13 October 2026

We discussed merge timing. Branch changes were already being brought together because other modules needed them. We also raised the risk of unfinished changes overwriting another contribution, especially in the shared layout, models, Program.cs and context/snapshot.

We needed to review affected shared files with their owners, preserve each contribution and rebuild the combined application. This did not mean every feature was already accepted, or that every merge should wait until the final day.

Action: Kamalpreet to coordinate the combined review; module owners to identify their shared model/configuration dependencies.

## M04 - VM requirement and locating Hold.cs

Dates: 13-14 October 2026

We checked that the installation guide must be tested on a VM through download, prerequisites, database setup and a working application for the selected branch. Harsimran owned the guide and VM check. Setup problems remained a blocker with no successful result.

Mehakpreet could not find Hold.cs on main. We clarified that it belonged to Kamalpreet's feature branch, and it was located on 14 October. Kiosk checkout needed the shared model rather than a second hold model. We also requested Sandesh's importer/report branch and delivery update that evening.

Actions: Harsimran to document/resolve the VM blocker; Mehakpreet to use the shared Hold model; Sandesh to supply the branch; Kamalpreet to coordinate integration.

## M05 - Branch, hold and migration contract

Date: not recorded in the retained technical follow-up; these decisions were used in the 14 October integration.

We aligned the shared data before generating another migration:

| Decision | Responsibility |
| --- | --- |
| Hold.PickupBranchId references Branch.Id. | Kamalpreet/Harsimran |
| InTransit uses value 5, preserving values 1-4. | Harsimran and module consumers |
| Checkout/API exclude transit and respect allocations. | Mehakpreet/Kamalpreet |
| Combine branch, hold, notification and patron Identity snapshots before migration. | Affected owners |
| Assign a valid branch on create, preserve it on ordinary edits and use transfers for moves. | Harsimran |

BranchId had become required while the existing Book/Music/Toy inputs did not provide it. We needed the old create/edit workflows to remain compatible.

## M06 - Final handoffs and corrections

Date: 15 October 2026

We restated the internal deadline and requested each member's task hours and own reflection, including a strength, difficulty, learning and improvement. Kamalpreet took responsibility for combining the portfolio and minutes.

We reviewed the importer/report handoff and kept its existing eight component cases. Before final acceptance we needed Quantity creating separate copies, valid publication years, hold/transit-aware availability, Darwin dates and loan-level fine details. We also needed a non-zero fine before/after payment rather than only zero-value summaries.

| Action | Owner | Target |
| --- | --- | --- |
| Correct import/report rules; supply samples, exports and final branch. | Sandesh | Internal handoff: 15 October |
| Resolve shared packages/layout/models while retaining working features. | Kamalpreet and owners | Before combined acceptance |
| Supply own task hours/reflection and identify estimates. | Every member | Final handoff |
| Compile portfolio, evidence and minutes. | Kamalpreet | Before submission |

## Action closeout - 16 October

This is a technical action update, not an additional meeting with assumed attendance.

| Action | Current position |
| --- | --- |
| Shared modules/schema | Combined release; seven canonical migrations retained. |
| Import/report corrections | Actual import captures, matching CSV/PDF and AUD 27 payment transition verified. |
| Combined tests | Windows build and 33 tests passed; all ten HTTPS API checks passed. Selected route denial and fresh-request logout matched expectations. |
| Individual handoffs | Four accounts received: 31 confirmed hours, 48/35 retrospective estimates and 30.76 partial reconstructed lead hours. |
| VM check | Still blocked during LocalDB installation; no successful or approved alternative recorded. |
| Submission | Final source push/review, portfolio upload and receipt remain release actions. |

We preserved decisions and actions without inventing a call location, duration, attendance list or missing acceptance date.
