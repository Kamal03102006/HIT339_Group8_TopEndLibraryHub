# Screenshot audit

Source checkpoint: a1e8190 on feature/holds-notifications, pushed following the 14 October circulation tests. All 21 repository images were accounted for. Seven were unchanged previously inspected images; fourteen new captures were inspected individually. Three existing historical uploads were restored. Descriptive filenames and the old-to-new mapping are in screenshots/README.md.

## Coverage checked

| Behaviour | Image evidence | Remaining limit |
| --- | --- | --- |
| Duplicate, FIFO and cancellation | Initial H01-H03 images retained. | Other eligibility/permission cases are separate. |
| Darwin return and collection | Noah Ready, pickup Email/SMS and Collected. | Wrong-member and one-active-loan checks remain reported. |
| Damaged copy and first repair | Damaged FIFO with Awaiting repair, borrowing rejection, Asha Allocated and zero early pickup records. | Exact database transactions are not inferred from UI images. |
| Protected branch handover | Wrong-destination error, origin-receipt error/In transit row, destination Received row. | Transit borrowing rejection image is absent; its filled form is labelled input-only. Repeat receipt is still separate. |
| Arrival and next family | Asha Ready/Noah Waiting, two Asha pickup channels, then Asha Collected/Noah Ready. | Later pickup-only totals are tester reports. |
| Second repair cycle | New Asha request #5 Waiting/Awaiting return before the return. | The later NeedsRepair-return state, repaired total six and restart six are reported rather than image-proven. |
| Earlier loan notifications | Restored 11 October loan-confirmation/due-soon and recheck photos. | Final fine/date/contact/permission and direct scheduler evidence remain separate. |

The two misleading original names were corrected to describe their visible states. No image was cropped, re-encoded or edited. No new screenshots are requested solely to recreate the already completed repair/restart check. If stronger proof is captured naturally during later integration tests, add it with its actual inputs and result.

## Remaining submission work

This audit concerns the supplied screenshots. Kiosk/API/concurrent checkout, importer/report handoff, clean-VM installation, genuine team/member records and final portfolio assembly are still outstanding in ASSESSMENT_CHECKLIST.md. Missing supplied evidence is not a claim that a teammate has done no work.

The subsequent [whole-commit review](PROJECT_PROGRESS_REVIEW.md) covers application source, migrations, all rubric statuses and remaining owner deliverables. Secure patron-facing hold placement is a requirement gap; the current creation form is staff-only.
