# Our agreement, approach and project plan

HIT339 Distributed Development | Group 8 | TopEndLibraryHub | 16 October 2026

## Our agreement

We selected Kamalpreet's Assignment 2 application as our common baseline. All four of us accepted the original agreement on the day it was created and shared. We did not retain its calendar date and have not assigned a later acceptance date.

We each take responsibility for implementing, explaining, testing and handing over our assigned modules. We work on feature branches and coordinate shared models, migrations, Identity and circulation changes. We preserve each member's work while resolving conflicts and distinguish component build/screenshots from final acceptance. We communicate availability, blockers and handoffs through our discussions and follow-ups. Kamalpreet coordinates dependencies, the release and portfolio.

We use GitHub issues and the project board to track tasks. We supply individual contribution/reflection and time records, identify estimates and keep private secrets in local configuration. Our acknowledgement records the assistance used in the lead's work.

| Member | Student number | Agreed responsibility |
| --- | --- | --- |
| Kamalpreet | S389008 | Lead; holds, simulated notifications, circulation integration, acceptance and portfolio. |
| Harsimran Singh | S388232 | Branches, desks, staff location, transfers, installation guide and VM check. |
| Mehakpreet Kaur | S390303 | Kiosk, secure catalogue API, patron guide and feedback. |
| Sandesh Bhattarai | S390535 | CSV importer, manager analytics/exports and component tests/handoff. |

## Our plan and actual progress

| Period | Planned work | Actual progress |
| --- | --- | --- |
| 3-4 October | Baseline, repository, board, roles and requirements. | We established the common project and allocations. |
| 10-12 October | Modules and shared dependencies. | Identity/Borrower, pickup branch and InTransit=5 coordinated; branch foundation preceded transfers. |
| 13-14 October | Integration and cross-module tests. | Branch evidence and hold, repair, transfer, collection/private-patron checks retained. |
| 15 October | Internal completion and handoffs. | Reports received; kiosk/API reached master. Import/report fixes continued later; VM/integration still open. |
| 16 October | One verified release and portfolio. | Combined source, 33 Windows tests, ten API checks, actual imports, report reconciliation and AUD 27 payment transition completed. Portfolio compiled; VM remains blocked. |

Our official deadline is 16 October 2026, 23:59 Darwin. We requested an extension but have no recorded approval or revised deadline.

Repository: https://github.com/Kamal03102006/HIT339_Group8_TopEndLibraryHub

Project board: https://github.com/users/Kamal03102006/projects/1

Both pages were publicly readable without authentication in the 16 October access check. We need to retain read access for marking and push the prepared final source.

## Dependencies and integration

We integrated Sandesh's corrections through da0f483 into the tested 9b28459 lead checkpoint, which already included kiosk/API and branch/transfer contributions. A direct merge produced 18 conflicts. We resolved shared files around one checkout service and one migration chain rather than accepting competing versions indiscriminately.

We retained seven canonical migrations, explicit Identity-to-Borrower ownership, two-minute kiosk handling and durable simulated events. We kept InTransit=5 and a real Branch foreign key for pickup. We aligned Identity key lengths with the committed schema and checked pending model changes/duplicate table creation.

Catalogue, API, kiosk and reports use common eligibility. A physical Available copy can be reserved; the rightful Ready member can collect while later members wait. Manager outputs share one service, keeping recorded paid/unpaid fines separate from current estimates. CSV Quantity creates separate physical copies with unique codes and row-specific errors.

## Allocations and hours

| Member | Total/subtotal | Basis | Detailed record |
| --- | ---: | --- | --- |
| Kamalpreet | 30.76 hours | Partial retrospective elapsed sessions; four-hour rule, through 16 Oct 19:29. | Individual account/session record. |
| Harsimran | 31 hours | Confirmed task hours; no daily start/end log supplied. | Individual ten-task table. |
| Mehakpreet | 48 hours | Revised retrospective estimate after overlap review; not measured actual time. | Individual nine-package table. |
| Sandesh | 35 hours | Member-supplied retrospective task estimate replacing earlier partial record. | Individual ten-task table. |

We count each total once and do not add unlike records into a precise team billable total. Assignment 2 baseline time and later assisted integration are not silently reassigned to another member.

## Risks and responses

| Risk | Response/result |
| --- | --- |
| Shared model/migration conflicts | Agreed data contract, canonical history, model/script and real SQL checks. |
| Reserved/transit stock shown as available | Shared eligibility, transaction/index guards and traceable inventory rows. |
| Late integration | One combined release and targeted acceptance; less time remained for VM recovery. |
| Incomplete daily logs | Confirmed hours, retrospective estimates and partial sessions kept distinct. |
| VM LocalDB failure | Preserved troubleshooting/failure record; no successful alternative claimed. |
| Final source visibility | Safe cumulative update prepared; owner still needs to push/review/merge before submission. |

## Our strengths and weaknesses

We covered seven modules and checked a connected repair-to-pickup journey. Named branches and retained evidence helped trace contributions, and exact messages/export rows helped explain confusing results.

Our weaknesses were late integration, uneven confidence with Git/shared models, incomplete daily time logs and arranging a usable VM too late. We would set data contracts, trial installation and schedule smaller integrations near the start next time.
