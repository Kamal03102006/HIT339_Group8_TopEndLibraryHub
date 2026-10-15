# Integration and user journeys

Prepared 14 October 2026 for the circulation patch. These diagrams describe the integrated source. Main circulation outcomes are reported in the test record; kiosk/API, concurrency and final seven-module acceptance remain pending. They are inputs to the required final portfolio, not the final PDF.

## Shared circulation

Reception and the authenticated kiosk call CirculationService. It validates member/location/item state, acquires the same transaction-owned per-item SQL application lock, refreshes the hold queue, and commits loan/item/hold changes together. Transfer dispatch/receipt and authorised stock edits use that item lock and HoldWorkflowService. A filtered unique active-loan index is the final duplicate guard.

Public catalogue, kiosk cards and API use CirculationRules for ordinary availability. Physical Available does not override an allocation. The API exposes catalogue projections only, behind an external API key. The notification worker catches up allocation and simulated messages every minute while the app runs, then source-state checks and a unique event/channel index protect its log.

The subsequent patron hold patch adds `HoldPlacementService`: staff creation resolves a membership number; `/Kiosk/Hold` resolves only the authenticated Identity link. Both recheck eligibility, pickup branch, duplicate requests and the member's active loan under the same per-item transaction. New requests start Waiting without a pickup message. No schema change is introduced. The new placement path has a supplied clean Windows build, reported PH01/PH02 outcomes and inspected inactive/unlinked GET/UI evidence. [The acceptance supplement](PATRON_HOLD_TEST_RECORD.md) distinguishes these from pending POST security, JavaScript-disabled expiry and final module checks.

## Pickup state transitions

```mermaid
stateDiagram-v2
    [*] --> Waiting
    Waiting --> Allocated: Usable copy, another pickup branch
    Waiting --> Ready: Usable copy at pickup branch
    Allocated --> Ready: Destination receives copy
    Ready --> Fulfilled: Correct member checks out
    Waiting --> Cancelled: Staff cancellation
    Allocated --> Cancelled: Staff cancellation
    Ready --> Cancelled: Staff cancellation
    Allocated --> Waiting: Member or pickup branch inactive
    Ready --> Waiting: Member or pickup branch inactive
    Fulfilled --> [*]
    Cancelled --> [*]
```

Cancelled/fulfilled requests remain history. Queue eligibility uses active member + active pickup branch, ordered CreatedAtUtc then Id. Damaged stock is allocated only after an authorised repair; in-transit stock is not ready. Cancellation at an unsuitable physical branch may allocate the next request for another protected transfer.

## Repair-to-pickup journey

| Stage | Person and goal | Action and feedback | Meaningful benefit | Evidence |
| --- | --- | --- | --- | --- |
| Find | Family wants a reusable toy | Search title/code, check condition and location. | Avoid a wasted visit for an unusable or reserved copy. | Final UI retest pending |
| Request | Patron wants fair access | A linked active patron selects Join waiting list and an active pickup branch. Staff can create an assisted request through the same service. The private account shows the member's own waiting position. | Members can request independently; duplicate requests cannot jump the queue. | Earlier H01–H02/FIFO images inspected; PH01/PH02 placement, duplicates and private progress reported; PH03 membership-blocking images inspected |
| Repair | Admin restores the damaged copy | Edit repaired condition to Available; queue head becomes Allocated. | Repair releases the existing queue without a second request. | C07 rejection and C08 allocation/zero-notice images inspected |
| Move | Reception delivers to pickup location | Dispatch from Darwin; receive only at Palmerston. | Reservation survives transport; no premature pickup message. | C09–C11 dispatch/receipt images inspected; transit borrowing rejection reported |
| Collect | Correct member wants the reserved toy | Linked kiosk or staff checkout fulfils the allocation. | Later members cannot take a reserved copy. | C12 staff Collected/next-member cards inspected; C15 kiosk pending |
| Reuse | Next waiting family wants access | Return in usable condition; next eligible request becomes Ready. | The queue continues fairly after borrowing. | C01 ready photo; C04–C06/C12 handover reported |

## Demonstrated lead journey

Kamalpreet created Asha and Noah holds on borrowed TY-0001 at Darwin. The duplicate was rejected; positions were Asha 1 / Noah 2. Cancelling Asha retained her history and promoted Noah to 1. Five screenshots at dbbc540 support those three UI outcomes. Those five images cover the initial desk case. The subsequent TY-9001 run reports repair, protected transfer, staff collection, next-family handover and a return requiring repair. Pickup totals progressed 0 → 2 → 4 → 6, then stayed six after restart. The 24-image checkpoint preserves 21 previously pushed captures plus three original historical observations. This supplement adds two PH03 photographs. For new Damaged TY-9901, the lead reports Sky's self-service Palmerston request, shared duplicate rejection, Sky 1 / Asha 2 and Sky-only private progress, with zero premature pickup notices. Inactive/unlinked membership blocking is visible in PH03 photographs; reactivation and Finish/Back are reported. The remaining kiosk checkout, API, POST security and other unrun cases remain pending.

## Other breadth features

Import and expanded management reports still await Sandesh's verified handoff. Their final integration diagram must identify actual routes, shared models, import validation and reporting reconciliation after the source is received. Do not present planned modules or draft PDF exports as implemented.
