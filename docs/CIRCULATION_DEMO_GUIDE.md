# Reproduce the circulation demonstration

This guide supplies a fresh-copy setup for the implemented staff circulation journey. The actual 14 October run used TY-9001; its results are in CIRCULATION_INTEGRATION_TEST_RECORD.md. Use the unused fictional copy TY-9101 below to preserve that historical evidence. These instructions are expected outcomes, not a claim that a fresh VM run has already passed.

## Prerequisites and fictional members

Complete README installation/migrations and sign in as the local Admin demonstration account. Startup seeds Darwin/DAR-D01, Palmerston/PAL-D01 and Casuarina/CAS-D01. It does not automatically create the manual Asha/Noah test members or TY-9101.

In Front desk → Manage borrowers, reuse these active members if present; otherwise create them with the following fictional fields. Preserve existing records and contact details if they already exist.

| Field | Asha | Noah |
| --- | --- | --- |
| Membership number | MBR-9001 | MBR-9002 |
| Full name | Asha Patel | Noah |
| Email | asha.holdtest@example.com | noah.holdtest@example.com |
| Phone | 0400000001 | 0400000002 |
| Address | 1 Demo Lane, Darwin NT 0800 | 2 Demo Lane, Darwin NT 0800 |

These are staff test borrowers. Private kiosk testing uses the separate Identity-linked demonstration member Sky / MBR-8001; do not claim a private account by entering Asha or Noah's membership number.

## Create the fresh toy at Darwin

Select Front desk → Working location → Darwin Hub / DAR-D01. Administration → Manage toys → Create:

| Field | Value |
| --- | --- |
| Library code | TY-9101 |
| Name | Top End Story Builders Kit — fresh demo |
| Description | Reusable family storytelling kit for the repair and branch-pickup demonstration |
| Toy type | Storytelling kit |
| Recommended age | 5–8 years |
| Manufacturer | Fictional demo supplier |
| Material | Wood and fabric |

The new item starts Available at the selected desk's branch. Edit its Status to Damaged and save. If TY-9101 already exists, choose another unused TY-#### code and use it consistently; do not delete earlier loans/holds to reset history.

## Queue repair and branch handover

1. Holds and waitlist → Place a hold: TY-9101 / MBR-9001 / pickup Palmerston. Repeat for MBR-9002. Expected: Asha then Noah in FIFO order. A duplicate active Asha request is rejected.
2. Attempt Borrow item at Darwin for the damaged copy. Expected: borrowing unavailable; no new loan.
3. Administration → Manage toys → Edit TY-9101 → Status Available → Save changes. Expected: Asha Allocated for Palmerston; no pickup messages yet because the copy is still at Darwin.
4. At DAR-D01, attempt transfer to Casuarina. Expected: wrong pickup destination rejected. Dispatch to Palmerston instead. Expected: In transit. Checkout and receipt at Darwin are rejected.
5. Select Palmerston / PAL-D01 and receive the transfer. Expected: Asha Ready for pickup; Noah remains waiting. Physical stock is now at Palmerston.
6. Notifications: search TY-9101, Message type Ready for pickup, Channel All channels, then Filter records. Expected: two records for Asha, one Email and one SMS.
7. Try Noah checkout first. Expected: the copy is reserved for Asha. Checkout for Asha with a due date 14 days after the displayed current Darwin date. Expected: one loan and Asha Collected.
8. Return the usable copy at PAL-D01 using today's displayed date, leaving Item needs repair unchecked. Expected: Noah Ready and two additional pickup records.

## Return requiring repair and recovery

1. Borrow the ready copy for Noah. Create a new Asha hold for the now-borrowed copy, pickup Palmerston.
2. Return the toy with Item needs repair checked. Expected: Damaged, Asha's new request Waiting, no early pickup notice. Earlier pickup messages remain history.
3. Record the completed repair through Manage toys → Edit → Available → Save changes. Expected: Asha's new request Ready and two new pickup records.
4. Stop/start the app, allow its automatic check to run for about one minute, then reapply the pickup filters. Expected: the history/count is unchanged.

| Point in the fresh-copy journey | Pickup-only records |
| --- | ---: |
| Damaged or allocated at Darwin, before destination receipt | 0 |
| First receipt at Palmerston, Asha Ready | 2 |
| Asha returns usable copy, Noah Ready | 4 |
| Noah returns with repair required, new Asha request Waiting | 4 |
| Staff record completed repair, new Asha request Ready | 6 |
| Restart with that same ready request | 6 |

Counts assume both fictional contacts above and no other prior history for the fresh item. Select Ready for pickup to exclude borrowing-confirmation, due-soon and fine messages. The immediate Check loans and pickups now button returns to an unfiltered page; reapply the item/type/channel filters before comparing totals. A new legitimate pickup event increases history; rechecking the same event must not.

## Record actual outcomes

Record the tested revision, actual date, desk, fields, result and available image/response references. Distinguish tester-reported results from inspected screenshots. This guide covers the staff repair/transfer journey; kiosk ownership/privacy, API authentication, simultaneous checkout, CSV/reports and clean-VM installation require their own actual checks.

## Current completion actions

This record preserves its tested checkpoint and stated evidence limits. The latest member handoffs and integration/VM actions are in [REMAINING_WORK.md](REMAINING_WORK.md). Documentation updates do not require repeating earlier tests solely to rename or index their evidence.
