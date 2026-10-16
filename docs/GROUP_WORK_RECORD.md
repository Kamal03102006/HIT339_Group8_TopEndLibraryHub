# Group 8 project work record

Project: TopEndLibraryHub. Unit: HIT339 Distributed Development. Updated 16 October 2026 for the combined release. The original agreement was accepted by all four members on the day it was created and shared; its calendar date is not recorded. Earlier progress logs remain in Git history at 9b28459.

## Team agreement

We selected Kamalpreet's Assignment 2 codebase as our shared foundation. Each member owns the implementation, explanation, evidence and handover of their assigned modules. Shared model, migration, identity and circulation changes require coordination. Work is developed on feature branches, reviewed and integrated; build success and component screenshots are distinguished from final acceptance. The group uses GitHub issues and the project board to track tasks, dependencies and unresolved work. Members communicate availability and blockers through the group channel and escalate integration issues to Kamalpreet.

Each member provides a contribution account and a time record with its basis. We retain actual test outcomes and limitations, preserve other members' work during conflict resolution, and keep user credentials out of committed configuration. AI assistance is disclosed separately in [the acknowledgement](AI_ACKNOWLEDGEMENT.md).

## Members and responsibilities

| Member | Student number | Responsibility | Handover position |
| --- | --- | --- | --- |
| Kamalpreet | S389008 | Team coordination, holds, notifications, shared circulation, integration and portfolio assembly | Working code and extensive observed/reported evidence; final Windows acceptance and PDF assembly remain |
| Harsimran Singh | S388232 | Branches, reception desks, transfers, installation guide and VM attempt | Report, seven historical component captures, reflection/value account and 31 confirmed hours received; successful VM verification remains blocked |
| Mehakpreet Kaur | S390303 | Self-service kiosk, secure catalogue API, patron guide and UX record | Component report, reflection and source received; revised hours proposal and final combined acceptance remain |
| Sandesh Bhattarai | S390535 | CSV importer, manager analytics and CSV/PDF exports | Source through da0f483, component report, reflection and eight captures received; final time entry and Windows handover remain |

## Project plan and actual milestones

| Period | Planned outcome | Actual progress and dependency |
| --- | --- | --- |
| 3-4 October | Establish the repository, task board, shared baseline and requirements | Foundation selected, roles discussed and requirements prepared |
| 10-12 October | Establish cross-module contracts and implement modules | Identity/Borrower link, hold/branch relationship and InTransit=5 coordinated; branch/transfer and hold/notification work progressed |
| 13-14 October | Integrate features and collect working results | Transfer component evidence captured; lead integrated circulation/patron safeguards and ran the documented hold/repair/transfer tests |
| 15 October | Internal completion target and member handovers | Reports and source handovers received; Mehak kiosk/API merged into master; Sandesh corrected importer/report code later that evening; final integration and VM verification remained open |
| 16 October | Complete the integrated release, verification and portfolio | Combined source, tests and updated records prepared. Current results and remaining work are in RELEASE_CHECKS_16OCT.md and REMAINING_WORK.md |

The official submission deadline is 16 October 2026 at 23:59 Darwin. An extension was requested; approval and a revised deadline are not recorded here. The original deadline remains the working target until written approval is received.

Project board: https://github.com/users/Kamal03102006/projects/1
Repository: https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub
Marker access must be checked using a signed-out session before submission.

## Task allocations and time records

The following records have different evidence bases and are not interchangeable. Do not add a member's subtotal again to the underlying tasks.

| Member | Recorded total or subtotal | Basis and remaining detail |
| --- | ---: | --- |
| Kamalpreet | Approximately 19.75 hours through 15 October 09:47 | Partial recovered elapsed-session estimate using her selected two-hour gap rule. Later/offline work and task-level splits are not included; see KAMALPREET_TIME_RECONSTRUCTION.md |
| Harsimran | 31 hours | Confirmed through Kamalpreet on 16 October. The ten-task breakdown is in individual/HARSIMRAN_SINGH.md; no daily start/end times were supplied |
| Mehakpreet | 48-hour proposal | Consolidated retrospective estimate awaiting member confirmation. The original 25 undated entries total 88, not the printed 85. See individual/MEHAKPREET_HOURS_REVIEW.md |
| Sandesh | 12.5-hour partial estimate | Earlier numerical entries supplied retrospectively; additional testing/documentation and later fixes are not yet timed. See SANDESH_MEMBER_HANDOFF.md |

Kamalpreet's allocated work includes requirements/coordination, hold models and queue rules, staff and patron placement, notifications, shared checkout/repair/transfer integration, interface improvements, evidence organisation, testing and documentation. Delivered work is linked in the existing circulation, patron and notification test records. The combined importer/report corrections and automated release checks on 16 October are integration work and are not silently counted as Sandesh's own hours.

Harsimran's ten-task record and Mehakpreet's nine-package proposal provide manageable task breakdowns. Sandesh's dated eight-row log records his earlier importer/report work. Original Assignment 2 development-log hours are baseline history and must not be added to this group project.

## Team approach and integration decisions

The integration starts from the lead's tested 9b28459 checkpoint, which already contains the branch/transfer foundation, Identity link, holds, notifications, patron placement and shared circulation services. Sandesh's quantity/year/import and export work through da0f483 is incorporated into that foundation. Common stock eligibility now serves catalogue, kiosk, API and management reports. Kiosk checkout calls CirculationService so a rightful pickup owner is not blocked solely because another member is waiting.

The seven-migration lead history is retained. The competing consolidated migration that creates already-existing tables is not added to that history. Identity key lengths are explicitly aligned with the committed schema, and the model/script test checks for pending model differences and duplicate shared-table creation. Existing database histories are checked before startup; a different historical schema requires a separate fresh demonstration database rather than deletion or blind migration-history editing.

Dashboard and export calculations share one reporting service. Recorded unpaid/paid fines and estimated current late charges are separate; exports contain traceable loan-level fine rows and item-level availability. The importer creates physical copies for Quantity and provides row-specific validation outcomes.

## Meeting minutes and individual sections

[Prepared meeting minutes](MEETING_MINUTES.md) consolidate the actual supplied discussion record and verbal-discussion follow-ups. They do not invent call start times, durations or full attendance. Recordings are not specified by the brief.

- [Kamalpreet](individual/KAMALPREET.md): contribution, reflection and partial time basis, with implementation evidence in the circulation/patron/notification records.
- [Harsimran Singh](individual/HARSIMRAN_SINGH.md): contribution, confirmed hours, strengths/weaknesses, learning and value added.
- [Mehakpreet Kaur](individual/MEHAKPREET_KAUR.md): supplied personal reflection, technical contribution and separate hours-review proposal.
- [Sandesh Bhattarai](SANDESH_MEMBER_HANDOFF.md): contribution, reflection, historical tests, source changes and partial time record.

All four individual accounts are received. The final portfolio still requires consolidation and final time-record confirmation; no member needs to recreate their reflection from scratch solely for formatting.
