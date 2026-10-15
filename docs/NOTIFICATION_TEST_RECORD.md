# Group 8 notification test and evidence record

Observed run: 11 October 2026. Record updated: 14 October 2026. Tester: Kamalpreet, using the Windows project. Times below are Darwin/ACST (UTC+09:30). Data belongs to fictional demonstration patrons.

Historical 11 October implementation was inspected at bcf6844; the combined circulation source is now verified at a1e8190. Results below distinguish historical photos, current inspected pickup images and user reports. Build and runtime outcomes come from the recorded Windows tests. Source inspections are listed separately. [AI acknowledgement](AI_ACKNOWLEDGEMENT.md).

## Observed results

| ID / requirement | Inputs and action | Expected result | Observed result | Evidence and limits |
| --- | --- | --- | --- | --- |
| N-OBS-01 / application and navigation | Run application after the holds/notification migration; sign in as Admin. | Existing application loads and authorized staff can navigate to Notifications. | **Observed:** manager dashboard loads; Notifications link is present. | Photo supplied at approximately 12:43, `image-1791688378936.jpg`. This does not prove a clean installation, other roles or all database constraints. |
| N-OBS-02 / repeated notification scan | Open `/Notifications` and use the guided repeated “Check active loans now” procedure. | An unchanged scan should not add the same event/channel again. | **Observed:** six matching simulated records and “Check complete. No new records were needed.” | Photo supplied at approximately 12:45, `image-1791688526957.jpg`. It supports an unchanged scan; a complete before/after count sequence and concurrent scans were not captured. |
| N-OBS-03 / N01 borrowing and N02 due soon | Guided procedure: borrow `BK-0001` for `MBR-0001`, due 12 October 2026; leave the app running for 60–90 seconds without a manual scan; filter by `bk-0001`. | Borrowed and due-soon messages use the correct borrower, item and date, with one record for each configured channel. | **Observed:** four matching rows: loan confirmed via Email and SMS; due soon via Email and SMS. Messages identify Discovering the Top End, `BK-0001`, due 12 October and “tomorrow.” All four show 11 October, 12:50:33 PM ACST. | Photo supplied at approximately 12:51, `image-1791688879276.jpg`. Message content and item filtering are visible. The guided procedure supports the automatic scan; the screenshot alone does not identify the triggering process. Capture a worker log or timed recording for direct scheduler evidence. |

The original 11 October photos have now been restored from the existing uploads under descriptive historical filenames. They are linked in [the screenshot index](screenshots/README.md). The dated observations above remain distinct from the later integrated tests.

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
| N-P09 / holds and branch pickup | Make the first queued copy ready at its chosen branch; verify the next patron's notice and absence of early/duplicate notifications. | **Observed and reported results.** Noah's Darwin pickup and Asha's Palmerston pickup each have inspected Email/SMS images; allocation with zero early messages is inspected. Recheck 2 → 2 and later repair/restart 6 → 6 remain reported. Repeat receipt, concurrent scans and exact-row recovery remain separate. |
| N-P10 / filters and paging | Check member/item code, message type and channel separately and together; create enough test records for multiple pages. Verify totals, ordering, empty results and navigation. | **Partial.** Item-code filtering observed in N-OBS-03; remaining checks pending. |

## Source review observations

- A background worker performs a startup check and schedules sequential checks every minute while the application runs.
- Each scan creates a service scope for the EF database context; simulations are stored in `NotificationLogs`.
- Event keys and a unique event/channel database index aim to prevent duplicate records. This requires runtime checks, especially for overlapping scans.
- Notification dates use a Darwin business date. Reminder amounts are estimated accruing fines; they are not proof of a payment or recorded manager revenue.
- The applied circulation patch calls hold-ready recording after return/repair/transfer allocation commits. Initial ready allocation and reported pickup counts are documented below; full journey, repeat/recovery and detailed message verification remain incomplete.
- Messages are simulations. Neither an actual email nor an actual SMS has been sent by this implementation.

## Record maintenance

Kamalpreet runs the guided checks and reports the actual behaviour. Written results are then compiled with the date, tester, revision, input, expected and observed outcome, and evidence reference. New hold-test completion reports are recorded in [HOLD_WORKFLOW_TEST_RECORD.md](HOLD_WORKFLOW_TEST_RECORD.md); they do not mark the outstanding notification checks as passed. Actual person-hours remain in [GROUP_WORK_RECORD.md](GROUP_WORK_RECORD.md).

## Integrated pickup check — 14 October evening

Noah MBR-9002 / TY-0001 / Darwin Hub is visibly Ready for pickup in the supplied c01_noah_ready_darwin.png screenshot. Following the guided filtered notification check, Kamalpreet reported at 20:30 Darwin that the Ready for pickup count stayed at two after a repeated immediate scan. This verifies the reported stable count for that scenario. Exact pickup-message content and Email/SMS pairing still await the log screenshot. Concurrent scans, restart and different-date checks remain pending. See C02/C03 in CIRCULATION_INTEGRATION_TEST_RECORD.md.

At 21:17 Darwin, Kamalpreet reported zero Ready for pickup records for repaired TY-9001 while Asha's hold was Allocated for Palmerston. This is reported absence of an early pickup notice before destination receipt; capture was subsequently reported complete, but the image has not been supplied.

At 21:49 Darwin, Kamalpreet confirmed the guided first receipt at Palmerston produced the expected Asha Ready state and two pickup simulations, one Email and one SMS. No screenshot was taken. This records a reported zero-before-arrival → two-after-arrival result, not independent inspection of message content or recipients. Current-screen captures were requested before collection; the manual recheck and unchanged count for this Palmerston event remain pending.

At 22:27 Darwin, after the collection/return guidance, the tester reported six notification results: two Noah rows and four Asha rows, with both Loan confirmed and pickup types among Asha's rows. The expected mixed-type explanation is four pickup rows plus two Asha borrowing-confirmation rows. Do not record this as six duplicate pickup messages or as a verified pickup-filter count. The next check explicitly selects TY-9001 / Ready for pickup / All channels and clicks Filter records; expected count four and repeat stability remain pending. The immediate CheckNow action in the applied source returns to an unfiltered page, so the guidance now explicitly reapplies filters after that action. Actual filter selection and exact channel/message content have not been independently inspected.

At 22:33 Darwin, the tester reported **4** following the pickup-only filter/recheck guidance. This matches the expected pickup-only total, with exact channel/message evidence and a separately stated before/after comparison still uninspected. The four-record pickup total is the starting history count for the subsequent return-requiring-repair check; historical ready notices should remain even while a new request waits for repair.

At 22:45 Darwin, Kamalpreet reported the return-requiring-repair batch matched expectations, including the pickup-only total remaining four while Asha's new request stayed Waiting. This is reported absence of an early ready notice during repair. The screenshot taken for this batch shows Waiting only; no notification-count image was supplied. A later completed repair should create a new legitimate Asha pickup event, increasing the historical pickup-only total to six. That next event and restart/recheck stability have not yet been reported.

At 23:07 Darwin, after guidance to save the completed repair by editing TY-9001 to Available, the tester reported **six**. This records the expected pickup-only increase from four to six by report; exact messages/channels and the Ready-card display are not separately image-inspected. The next restart check should retain six without generating duplicates; its actual outcome is pending.

At 23:15 Darwin, the tester reported **six after stop/start**, compared with six before restart. This is a **reported pickup-history/count stability pass, 6 → 6 on the same Darwin date**. No additional screenshot was requested. It does not mark the separate loan-reminder restart, different-date fine update, concurrent scan or exact event/channel checks passed.


## Screenshot inspection after pushed checkpoint a1e8190

All 21 pushed image files have now been inspected or matched to previously inspected bytes and are indexed in [screenshots/README.md](screenshots/README.md). Earlier dated notes that images were local or awaiting filenames are historical. The audit upgrades only outcomes actually visible: damaged FIFO, Asha allocation, zero early pickup records, wrong-destination/origin-receipt errors, first receipt, correct pickup channels and collection/next-member cards. The transit checkout form contains no visible rejection, and the new Asha #5 Waiting capture says Awaiting return. The later NeedsRepair-return and repaired/restart counts remain tester reports. See [the audit](SCREENSHOT_AUDIT.md) for scope and gaps.

The new inspection observes two Noah TY-0001 pickup records (Email/SMS, Darwin, 20:11:36) and two Asha TY-9001 pickup records (Email/SMS, Palmerston, 21:49:02 on 14 October). These images now verify recipients, channel pairing and pickup-branch content for those generations. They do not supply the later four/six-record recheck sequence or concurrent scans.

## Current completion actions

This record preserves its tested checkpoint and stated evidence limits. The latest member handoffs and integration/VM actions are in [REMAINING_WORK.md](REMAINING_WORK.md). Documentation updates do not require repeating earlier tests solely to rename or index their evidence.
