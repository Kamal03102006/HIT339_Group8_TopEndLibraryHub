# Screenshot index and captions

This README.md is a text index, not a screenshot. It explains what each image demonstrates and links it to the test record.

The audit verified the 21 images pushed in a1e8190. Fourteen were newly inspected; seven matched previously inspected files byte for byte. There were no exact duplicate files. All 21 are retained under descriptive names with their image bytes unchanged. Three historical 11 October notification photos were restored from existing uploads, giving 24 evidence images.

## Current hold and circulation evidence

| Test | Image | What is actually visible |
| --- | --- | --- |
| H01 | [h01_duplicate_hold_rejected.png](h01_duplicate_hold_rejected.png) | Duplicate Asha / MBR-9001 / TY-0001 request rejected. |
| H02 | [h02_fifo_queue_overview.png](h02_fifo_queue_overview.png) | Initial borrowed-copy queue overview; use with the detail capture. |
| H02 | [h02_fifo_queue_details.png](h02_fifo_queue_details.png) | Asha position 1 and Noah position 2 for TY-0001 at Darwin. |
| H03 | [h03_cancelled_hold_queue_overview.png](h03_cancelled_hold_queue_overview.png) | Cancellation result and Noah promoted to waiting position 1. |
| H03 | [h03_cancelled_hold_history.png](h03_cancelled_hold_history.png) | Complementary capture retaining Asha's Cancelled history. |
| C01 | [c01_noah_ready_darwin.png](c01_noah_ready_darwin.png) | Noah request #2 / TY-0001 Ready for pickup at Darwin. |
| C02 | [c02_noah_pickup_email_sms_darwin.png](c02_noah_pickup_email_sms_darwin.png) | Two Noah / TY-0001 pickup records, Email and SMS, naming Darwin; created 14 October 20:11:36. |
| C05 | [c05_noah_collected_darwin.png](c05_noah_collected_darwin.png) | Noah request #2 / TY-0001 Collected at Darwin. Does not display an active-loan count. |
| C07 | [c07_damaged_toy_checkout_rejected.jpg](c07_damaged_toy_checkout_rejected.jpg) | Borrow form rejects TY-9001 / MBR-9001 due 28 October. Damaged setup is documented separately. |
| C07 | [c07_damaged_toy_fifo_waiting.png](c07_damaged_toy_fifo_waiting.png) | Initial damaged-copy queue: Asha #3 position 1, Noah #4 position 2, both Awaiting repair. |
| C08 | [c08_asha_allocated_palmerston.png](c08_asha_allocated_palmerston.png) | Asha #3 Allocated for Palmerston; Noah #4 still Waiting at position 1. |
| C08 | [c08_no_early_pickup_notifications.png](c08_no_early_pickup_notifications.png) | TY-9001 / Ready for pickup / All channels shows zero matching records before arrival. |
| C09 | [c09_wrong_pickup_destination_rejected.png](c09_wrong_pickup_destination_rejected.png) | Dispatch to Casuarina rejected: reserved copy must travel to its selected pickup branch. Origin DAR-D01. |
| C10 | [c10_origin_receipt_rejected_in_transit.png](c10_origin_receipt_rejected_in_transit.png) | Receipt attempted at DAR-D01 rejected; TY-9001 remains In transit from Darwin to Palmerston. |
| C10 input only | [c10_checkout_form_before_submit.png](c10_checkout_form_before_submit.png) | Filled TY-9001 / MBR-9001 checkout form at Darwin, due 28 October. No rejection message is visible. |
| C11 | [c11_transfer_received_palmerston.png](c11_transfer_received_palmerston.png) | PAL-D01 transfer row Received/Completed; dispatched 14 October 21:27, received 21:49. |
| C11 | [c11_asha_ready_noah_waiting_palmerston.png](c11_asha_ready_noah_waiting_palmerston.png) | Asha #3 Ready at Palmerston while Noah #4 remains Waiting at position 1. |
| C11 | [c11_asha_pickup_email_sms_palmerston.png](c11_asha_pickup_email_sms_palmerston.png) | Two Asha / TY-9001 pickup simulations, Email and SMS, naming Palmerston; created 14 October 21:49:02. |
| C12 | [c12_asha_collected_noah_ready_palmerston.png](c12_asha_collected_noah_ready_palmerston.png) | Both cards: Asha #3 Collected and Noah #4 Ready at Palmerston after the guided usable return. |
| C12 | [c12_noah_ready_palmerston.png](c12_noah_ready_palmerston.png) | Complementary capture of Noah #4 Ready at Palmerston. |
| C13 setup | [c13_new_asha_hold_awaiting_return.png](c13_new_asha_hold_awaiting_return.png) | New Asha request #5, requested 14 October 22:41, Waiting position 1 and Awaiting return. This is before the damaged return. |

## Historical notification evidence

These three images show the earlier 11 October interface and data. They support the dated observations; they are not labelled as screenshots of the final integrated version.

| Test | Image | What is actually visible |
| --- | --- | --- |
| N-OBS-01 | [n_obs01_historical_dashboard_11oct.jpg](n_obs01_historical_dashboard_11oct.jpg) | 11 October historical dashboard with Notifications link; baseline statistics, not expanded manager exports. |
| N-OBS-02 | [n_obs02_historical_notification_recheck_11oct.jpg](n_obs02_historical_notification_recheck_11oct.jpg) | 11 October historical notification page: six records and Check complete / no new records needed. |
| N-OBS-03 | [n_obs03_historical_borrow_due_soon_11oct.jpg](n_obs03_historical_borrow_due_soon_11oct.jpg) | 11 October BK-0001 / MBR-0001: loan-confirmation and due-soon messages through Email and SMS, due 12 October. |

## Evidence gaps and agreed written results

- The transit checkout image shows the input form, not an unavailable/rejection message. The C10 rejection remains a reported result.
- The new Asha request #5 image says Awaiting return. It documents the next-cycle setup, not the later damaged-return outcome. The initial #3/#4 damaged queue image does show Awaiting repair.
- Wrong-member rejection and repeat checkout with one active loan remain reported where no result image was supplied. The Collected image alone does not prove the active-loan count.
- The later four/six pickup totals and 6 → 6 restart result remain written tester reports. No additional screenshots were requested for that repair/restart check; its absence is not a new screenshot task.
- Kiosk/API/privacy, CSV import, expanded exports and clean-VM evidence belong to their remaining tests/member handoffs. The available rubric does not prescribe an image for every click or test.

See [the screenshot audit](../SCREENSHOT_AUDIT.md), [circulation results](../CIRCULATION_INTEGRATION_TEST_RECORD.md), [hold results](../HOLD_WORKFLOW_TEST_RECORD.md) and [notification results](../NOTIFICATION_TEST_RECORD.md). Selected figures and these captions will be included in the final portfolio.

## Original filename mapping

This table preserves the identity of the originally named files after renaming.

| Original filename | Current filename |
| --- | --- |
| Duplicate-request error.png | [h01_duplicate_hold_rejected.png](h01_duplicate_hold_rejected.png) |
| position 1 and 2.png | [h02_fifo_queue_overview.png](h02_fifo_queue_overview.png) |
| p1 and p2.png | [h02_fifo_queue_details.png](h02_fifo_queue_details.png) |
| cancellation.png | [h03_cancelled_hold_queue_overview.png](h03_cancelled_hold_queue_overview.png) |
| cancellation1.png | [h03_cancelled_hold_history.png](h03_cancelled_hold_history.png) |
| Ready to pickup.png | [c01_noah_ready_darwin.png](c01_noah_ready_darwin.png) |
| notifications.png | [c02_noah_pickup_email_sms_darwin.png](c02_noah_pickup_email_sms_darwin.png) |
| collected.png | [c05_noah_collected_darwin.png](c05_noah_collected_darwin.png) |
| 1000061793.jpg | [c07_damaged_toy_checkout_rejected.jpg](c07_damaged_toy_checkout_rejected.jpg) |
| TY-9001 Waiting.png | [c07_damaged_toy_fifo_waiting.png](c07_damaged_toy_fifo_waiting.png) |
| ALLOCATED.png | [c08_asha_allocated_palmerston.png](c08_asha_allocated_palmerston.png) |
| 0 RECORD.png | [c08_no_early_pickup_notifications.png](c08_no_early_pickup_notifications.png) |
| DISPATCH.png | [c09_wrong_pickup_destination_rejected.png](c09_wrong_pickup_destination_rejected.png) |
| INTERBRANCH TRANSFERS.png | [c10_origin_receipt_rejected_in_transit.png](c10_origin_receipt_rejected_in_transit.png) |
| BRANCH RECEIVE BEFORE CHECKOUT.png | [c10_checkout_form_before_submit.png](c10_checkout_form_before_submit.png) |
| TRANSIT COMPLETED.png | [c11_transfer_received_palmerston.png](c11_transfer_received_palmerston.png) |
| HOLD.png | [c11_asha_ready_noah_waiting_palmerston.png](c11_asha_ready_noah_waiting_palmerston.png) |
| NOTIFICATION TY-9001.png | [c11_asha_pickup_email_sms_palmerston.png](c11_asha_pickup_email_sms_palmerston.png) |
| COLLECTED-ASHA.png | [c12_asha_collected_noah_ready_palmerston.png](c12_asha_collected_noah_ready_palmerston.png) |
| READY-NOAH.png | [c12_noah_ready_palmerston.png](c12_noah_ready_palmerston.png) |
| ASHA WAITING.png | [c13_new_asha_hold_awaiting_return.png](c13_new_asha_hold_awaiting_return.png) |
| 01-image-1791688378936.jpg | [n_obs01_historical_dashboard_11oct.jpg](n_obs01_historical_dashboard_11oct.jpg) |
| 01-image-1791688526957.jpg | [n_obs02_historical_notification_recheck_11oct.jpg](n_obs02_historical_notification_recheck_11oct.jpg) |
| 01-image-1791688879276.jpg | [n_obs03_historical_borrow_due_soon_11oct.jpg](n_obs03_historical_borrow_due_soon_11oct.jpg) |


## Patron membership access evidence — 15 October

The four patron captures below are present and inspected in pushed **6a69ade**. The PH03 photograph bytes remain unchanged; PH01 and PH02 were supplied in the patron push under the requested descriptive filenames. These supplement the earlier 24-image checkpoint, giving **28 repository evidence images**.

| Test | Image | What is actually visible |
| --- | --- | --- |
| PH01 request | [ph01_sky_waiting_palmerston.png](ph01_sky_waiting_palmerston.png) | Success for MBR-8001 / TY-9901 / Palmerston; Sky Active with no active loans and one visible Waiting request, position 1. Taken before PH03; zero notices remain reported. |
| PH02 queue | [ph02_shared_queue_sky_asha.png](ph02_shared_queue_sky_asha.png) | TY-9901 staff queue: Sky #6 at position 1, Asha #7 at position 2, both Waiting/Palmerston/awaiting repair. Does not picture the duplicate submissions or private-account check. |
| PH03 inactive | [ph03_sky_inactive_hold_blocked.jpg](ph03_sky_inactive_hold_blocked.jpg) | /Kiosk/Account shows Sky Nguyen / MBR-8001 inactive, placement blocked, no active loans, and a paused Wet Season Story Kit request. The item code is below the captured portion. Does not show reactivation. |
| PH03 unlinked | [ph03_unlinked_hold_blocked.jpg](ph03_unlinked_hold_blocked.jpg) | Placement requires an active linked membership; account is not linked. No personal member details or creation form is displayed. The signed-in identity/role is not visible. |

| Original upload | Prepared evidence filename |
| --- | --- |
| 01-image-1792021443704.jpg | ph03_sky_inactive_hold_blocked.jpg |
| 02-image-1792021518320.jpg | ph03_unlinked_hold_blocked.jpg |

See [the patron hold acceptance record](../PATRON_HOLD_TEST_RECORD.md). Repeated waiting tests are not requested to replace existing timeout observations.

## Sandesh component evidence received on 15 October

Eight original component captures are added under screenshots/sandesh. The earlier 28 images are unchanged, giving 36 retained test captures in this update. [The member handoff](../SANDESH_MEMBER_HANDOFF.md) maps the results, original captions and evidence limits. The SQL output shows BranchId, not Branched; the catalogue image visibly retains Year 0. These captures do not close the outstanding integrated, access-control or non-zero-fine checks.

- [sd_tc01_bulk_100_imported.png](sandesh/sd_tc01_bulk_100_imported.png)
- [sd_tc02_duplicate_100_skipped.png](sandesh/sd_tc02_duplicate_100_skipped.png)
- [sd_tc03_one_row_skipped.png](sandesh/sd_tc03_one_row_skipped.png)
- [sd_tc04_missing_headers.png](sandesh/sd_tc04_missing_headers.png)
- [sd_tc05_catalogue_bk0102_year_zero.png](sandesh/sd_tc05_catalogue_bk0102_year_zero.png)
- [sd_tc06_csv_summary.png](sandesh/sd_tc06_csv_summary.png)
- [sd_tc07_pdf_summary.png](sandesh/sd_tc07_pdf_summary.png)
- [sd_tc08_branch_sql_result.png](sandesh/sd_tc08_branch_sql_result.png)
