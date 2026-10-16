# Group 8 Team Discussion Minutes

HIT339 Distributed Development  |  TopEndLibraryHub

Prepared retrospectively on 15 October 2026. Recorder: Kamalpreet S389008.

Group members: Kamalpreet, Harsimran Singh, Mehakpreet Kaur and Sandesh Bhattarai.

These minutes summarise the discussions confirmed by Kamalpreet and their recorded follow ups. Dates identify the follow ups; exact meeting start and finish times and complete attendance at preceding conversations were not recorded. Named contributors are identifiable in the retained records.

## M01 Project scope and allocation

Recorded follow up dates: 3 and 4 October 2026

Contributors identified: All four members

Retained reference: Group record pages 1 and 2

Discussion and direction. The group would extend Kamalpreet's Assignment 2 application. The proposed demonstration follows an unavailable toy through repair, fair allocation, transfer to the patron's chosen branch and collection through the kiosk. The allocation covers all seven technical additions.

| Member | Technical work | Supporting delivery |

| --- | --- | --- |

| Kamalpreet | Holds, waiting lists and simulated notifications | Requirements, coordination and integration evidence |

| Harsimran | Branches, desks and item transfers | Installation guide and VM verification |

| Mehakpreet | Kiosk and secure read only API | Patron guide, feedback and API tests |

| Sandesh | CSV import and manager exports | Import/report instructions, test data and portfolio sections |

Actions. Everyone to clone, build and run the shared project, review the requirements and confirm responsibilities/availability. Harsimran to deliver the branch foundation first. Sandesh confirmed accepting the repository invitation on 4 October. Every member to retain their own contribution, hours and testing evidence.

## M02 Schedule and dependencies

Recorded follow ups: 10 to 12 October 2026. Contributors: all four members. Reference: group record pages 2 to 5.

The internal completion target was 15 October, with the official deadline on 16 October. The initial feature target was 13 October; the later 11 October handoff request aimed for working modules by 14 October. Integration and documentation were reserved for 14 to 15 October.

Actions. Harsimran to share models, location fields, migrations and seed data before finishing transfers. Mehakpreet to coordinate patron identity and checkout guards with holds/transfers. Sandesh to define CSV branch mapping and distinguish unpaid fines from collected revenue. On 12 October Harsimran reported the foundation running and transfers still pending.

## M03 Merge practice and shared files

Recorded follow up dates: 13 October 2026

Contributors identified: Kamalpreet, Harsimran and Mehakpreet

Retained reference: Group record page 5

Discussion. Harsimran reported that branch work had already been merged. Mehakpreet raised concern that merging unfinished shared changes could create conflicts or lose another contribution. Harsimran explained that other members needed current code and gave shared layout edits as an example. Kamalpreet acknowledged the need to connect the branches.

Outcome. The exchange establishes different views about merge timing. It does not record an agreed rule to delay every merge until the end, or a completed final integration. Shared files and database changes needed explicit coordination.

Follow up. Kamalpreet and the module owners to review the shared changes before the final merge, retain each member's contribution and rebuild the combined application. The project file, navigation, models and database history are integration points requiring review.

## M04 VM testing and missing hold model

Recorded follow up dates: 13 and 14 October 2026

Contributors identified: Kamalpreet, Harsimran, Mehakpreet and Sandesh

Retained reference: Group record pages 5 to 7

Discussion. Harsimran asked about the VM requirement. The recorded scope included downloading the application, installing prerequisites, building and creating the database/seed data, selecting a branch and desk, and completing a transfer. Kamalpreet confirmed on 14 October that VM testing was required. The exact virtualisation product was not settled in that exchange.

Harsimran subsequently reported problems with VM testing. This was recorded as a blocker; no successful VM installation was established by that update. He was to continue the setup and supply a usable installation guide and verification result.

A separate dependency was resolved when Mehakpreet could not locate Hold.cs in the main branch. Ownership was clarified as Kamalpreet's. Kamalpreet identified the model in her feature branch, and Mehakpreet confirmed that she found it on 14 October.

Sandesh was asked for his progress and said that he would provide the completed work that evening. The final importer/report handoff was still awaited at that point.

Actions. Harsimran to resolve and document the VM blocker. Mehakpreet to use the shared Hold model when connecting checkout. Sandesh to provide his branch and delivery update. Kamalpreet to coordinate the shared dependencies.

## M05 Branch and hold data contract

Recorded follow up dates: Calendar date not visible in the retained screenshot

Contributors identified: Kamalpreet and Harsimran

Retained reference: Private technical follow up screenshot supplied on 15 October

The retained screenshot labels the reply Tuesday without displaying a calendar date. It records Kamalpreet's response to the branch foundation and the following technical alignment:

| Technical point | Recorded direction |

| --- | --- |

| Pickup location | Hold.PickupBranchId references Branch.Id. |

| Transit status | ItemStatus.InTransit uses value 5, preserving values 1 to 4. |

| Checkout and API | Exclude items in transit and respect existing hold allocations. |

| Migrations and item location | Coordinate the branch, hold, notification and patron identity model snapshots. Provide a valid branch on item creation, preserve location during ordinary edits and use transfers for moves. |

Actions. Harsimran to handle the item creation/edit compatibility issue and coordinate the next migration. Kamalpreet to coordinate the hold/notification integration with Mehakpreet's identity work. This record confirms the data contract; it is not a record of completed runtime tests.

## M06 Final handoffs and correction review

Recorded follow up dates: 15 October 2026

Contributors identified: Kamalpreet and Sandesh; requests addressed to all members

Retained reference: Group record pages 7 to 11

Discussion. Kamalpreet restated the 15 October internal deadline and requested dated task hours and individual reflections from every member. Approximate past hours were to be labelled as estimates. Each reflection was to cover contribution, strength, difficulty, learning and improvement. Kamalpreet took responsibility for minutes and combining the named sections.

Sandesh supplied his report and reported a pushed importer/reporting branch and PR 7. Review of 2ab17fa confirmed the features existed. His eight local test cases, screenshots and reflection were accepted as usable component evidence, with corrections required before final integration.

Requested corrections. Quantity must create the requested uniquely coded copies; publication year must be valid. Reports need transit and hold aware availability, Darwin dates and traceable fine details. Further outcomes were requested for quantity/year, access restrictions and non-zero fines before and after payment. Sandesh was to supply sample CSVs, actual exports, operating instructions, a revised hours total and a final build/commit.

Integration action. Sandesh and Kamalpreet to resolve project package and shared layout conflicts together, retaining the existing dependency fix and navigation. Kamalpreet to assemble the confirmed member material and final formatting. The 15:12 follow up retained the existing evidence and requested new results for the corrected revision.

## Action register

This register consolidates the recorded requests. Delivery or discussion alone is not treated as final integration acceptance. The source period ends with the 15 October follow up at 15:12; later changes are tracked in the project records.

| ID | Action and owner | Recorded position and target |

| --- | --- | --- |

| A01 | All members: work from the shared base and retain identifiable contributions. | Assigned 3 to 4 October. Confirm operation again on the final release. |

| A02 | Harsimran: branch/desk foundation, item locations, seeds and transfer handoff. | Foundation reported running 12 October. Final transfer integration remains a group check. |

| A03 | Kamalpreet and Mehakpreet: align identity, holds, availability and checkout. | Shared dependency identified 10 to 11 October. Hold model located 14 October. |

| A04 | Kamalpreet and Harsimran: preserve the branch and hold data contract. | Technical directions retained in M05. Review shared snapshots before another migration. |

| A05 | Harsimran: finish VM installation/run verification and update the guide. | VM problems reported 14 October. Successful result not established in these minutes. |

| A06 | Sandesh: quantity/year fixes, inventory and fine reporting, Darwin dates. | Requested 15 October after source/report review. Target: 15 October. |

| A07 | Sandesh: targeted tests, sample inputs, actual exports and operating instructions. | Keep the eight existing cases; append corrected revision outcomes. Target: 15 October. |

| A08 | All members: dated task hours and individual contribution/reflection. | Requested 15 October. Label estimates and remove overlapping time; confirm member totals. |

| A09 | Kamalpreet: compile minutes and combine the named portfolio sections. | Responsibility confirmed 15 October. These retrospective minutes prepared that day. |

| A10 | Kamalpreet with all owners: resolve shared files and test one combined release. | Required before the final portfolio and submission. Preserve working features and dependency fixes. |

## Sources and record scope

Reference A: supplied group project discussion export, 11 pages, covering 20 September to 15 October 2026. Project decisions used here begin on 3 October. Page references in the minutes point to this retained export.

Reference B: supplied Harsimran technical follow up screenshot. Its weekday label and visible content support M05; the calendar date is not displayed. Private contact details and unrelated conversation are omitted from these minutes.

The group agreement acceptance is maintained separately. The export shows role allocation, repository access and planning acknowledgements; these do not establish an original calendar date for every member's contract acceptance.
