# Group 8 notification test and evidence record

Observed run: 11 October 2026. Record updated: 14 October 2026. Tester: Kamalpreet, using the Windows project. Times below are Darwin/ACST (UTC+09:30). Data belongs to fictional demonstration patrons.

Implementation inspected at commit `bcf6844` on `feature/holds-notifications`. Results below come from the user's guided local run and supplied screenshots. The assistant did not independently compile the project or run SQL Server. Source review is kept separate from runtime results.

## Observed results

| ID / requirement | Inputs and action | Expected result | Observed result | Evidence and limits |
| --- | --- | --- | --- | --- |
| N-OBS-01 / application and navigation | Run application after the holds/notification migration; sign in as Admin. | Existing application loads and authorized staff can navigate to Notifications. | **Observed:** manager dashboard loads; Notifications link is present. | Photo supplied at approximately 12:43, `image-1791688378936.jpg`. This does not prove a clean installation, other roles or all database constraints. |
| N-OBS-02 / repeated notification scan | Open `/Notifications` and use the guided repeated “Check active loans now” procedure. | An unchanged scan should not add the same event/channel again. | **Observed:** six matching simulated records and “Check complete. No new records were needed.” | Photo supplied at approximately 12:45, `image-1791688526957.jpg`. It supports an unchanged scan; a complete before/after count sequence and concurrent scans were not captured. |
| N-OBS-03 / N01 borrowing and N02 due soon | Guided procedure: borrow `BK-0001` for `MBR-0001`, due 12 October 2026; leave the app running for 60–90 seconds without a manual scan; filter by `bk-0001`. | Borrowed and due-soon messages use the correct borrower, item and date, with one record for each configured channel. | **Observed:** four matching rows: loan confirmed via Email and SMS; due soon via Email and SMS. Messages identify Discovering the Top End, `BK-0001`, due 12 October and “tomorrow.” All four show 11 October, 12:50:33 PM ACST. | Photo supplied at approximately 12:51, `image-1791688879276.jpg`. Message content and item filtering are visible. The guided procedure supports the automatic scan; the screenshot alone does not identify the triggering process. Capture a worker log or timed recording for direct scheduler evidence. |

The original photos were supplied in the conversation and their observed content remains recorded here. Kamalpreet reports that the old photos are not retained among the three current hold-test screenshots on the laptop. They are **not attached repository assets**. The current screenshot collection can begin with the three hold checks. During the final integrated notification checks, capture only the evidence needed to demonstrate the final implementation; there is no need to recreate an old interface photograph merely to reproduce its historical filename.

## Remaining runtime checks

No test in this table is marked passed until its actual result and evidence are recorded.

| ID / requirement | Procedure and expected result | Status / actual result |
| --- | --- | --- |
| N-P01 / accruing fines | Filter `TY-0001` and Fine accruing with All channels. Check the displayed overdue days and estimated fine against the agreed $1.50 per overdue day. Retain the loan due date and message. | **Pending evidence.** Test already requested; no result supplied. |
| N-P02 / persistence and restart | Note the total and newest rows; stop and restart the application; reopen Notifications and run a scan. For unchanged loans within the same Darwin date, history remains and no same event/channel is duplicated. | **Pending evidence.** Test already requested; no result supplied. |
| N-P03 / due window boundaries | Use separate test loans due in 3 days, 2 days, today and yesterday. Verify that only the agreed near-due range receives a due-soon event and that an overdue loan receives the appropriate fine event. | **Pending.** Record exact test dates. |
| N-P04 / returned loan | Return a test loan before a scan, then run the scan. No new borrowed/due/fine event should be created for that returned loan. Existing history should remain. | **Pending.** |
| N-P05 / next business day | In an isolated test environment using a controlled clock, repeat a scan on the same Darwin date and then the next date. Fine reminders are unique per date/channel; estimated days and amount advance correctly. | **Pending.** Do not alter the shared computer's clock as a substitute for a controlled test. |
| N-P06 / access control | Visit the route signed out and with each available staff role. Allowed staff can review logs; unauthorized users cannot read recipients or messages. Verify the manual scan requires a valid authenticated form submission. | **Pending.** Source attributes alone are not a runtime pass. |
| N-P07 / channels and contact details | Use fictional borrowers with email only, mobile only, both and neither. Check generated channel counts and absence of invalid recipient rows. | **Pending.** |
| N-P08 / duplicate/concurrent scans | Trigger overlapping scans in a controlled test. Count records by event key and channel. There must be at most one of each and no unhandled error. | **Pending.** Database constraints are implemented but this scenario has not been run. |
| N-P09 / holds and branch pickup | After the hold workflow is integrated, make the first queued item ready at the chosen branch. Verify the correct patron receives the pickup notice, no later patron receives it early, and repeat actions do not duplicate it. | **Blocked by workflow integration.** |
| N-P10 / filters and paging | Check member/item code, message type and channel separately and together; create enough test records for multiple pages. Verify totals, ordering, empty results and navigation. | **Partial.** Item-code filtering observed in N-OBS-03; remaining checks pending. |

## Source review observations

- A background worker performs a startup check and schedules sequential checks every minute while the application runs.
- Each scan creates a service scope for the EF database context; simulations are stored in `NotificationLogs`.
- Event keys and a unique event/channel database index aim to prevent duplicate records. This requires runtime checks, especially for overlapping scans.
- Notification dates use a Darwin business date. Reminder amounts are estimated accruing fines; they are not proof of a payment or recorded manager revenue.
- A hold-ready recording method exists, but the complete hold/branch workflow has not yet called and demonstrated it.
- Messages are simulations. Neither an actual email nor an actual SMS has been sent by this implementation.

## Record maintenance

Kamalpreet runs the guided checks and reports the actual behaviour. Written results are then compiled with the date, tester, revision, input, expected and observed outcome, and evidence reference. New hold-test completion reports are recorded in [HOLD_WORKFLOW_TEST_RECORD.md](HOLD_WORKFLOW_TEST_RECORD.md); they do not mark the outstanding notification checks as passed. Actual person-hours remain in [GROUP_WORK_RECORD.md](GROUP_WORK_RECORD.md).
