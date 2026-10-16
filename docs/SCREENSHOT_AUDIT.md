# Screenshot audit

Current evidence set: **48 retained captures**: 28 lead images, seven Harsimran component captures, eight Sandesh component captures and five actual Windows import/dashboard captures from 16 October. The earlier counts below describe their dated checkpoints. The current release results are in [RELEASE_CHECKS_16OCT.md](RELEASE_CHECKS_16OCT.md).

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

## Final release verification

The final release passed 33 Windows tests, including real SQL circulation cases, and ten HTTPS API checks. The actual browser imports, manager CSV/PDF and AUD 27 payment transition were inspected. Four individual records and the portfolio are complete. The separate VM installation remains blocked; deeper browser security checks remain outside the completed scenarios.

The subsequent [whole-commit review](PROJECT_PROGRESS_REVIEW.md) covers application source, migrations, all rubric statuses and remaining owner deliverables. The earlier staff-only patron-placement gap was addressed by source commit 70db5b7. The subsequent PH01–PH03 inspection is recorded below; remaining POST/privacy checks are separate.

## Patron supplement and verified push — 15 October

Latest inspected source/evidence revision: **6a69ade**, including patron update **70db5b7**. All **28** images are present: the earlier 24 retain their bytes, and four patron captures are indexed under descriptive names. PH01 visibly confirms Sky / MBR-8001, TY-9901 Waiting position 1 and Palmerston pickup after successful placement. PH02 shows Sky request #6 at position 1 and Asha request #7 at position 2, both Waiting and awaiting repair. PH03 shows inactive and unlinked GET/UI access blocking.

The queue image does not picture duplicate rejection, and the account capture does not independently prove every private-data test. Those actions remain tester reports. PH01 predates PH03 and is not a reactivation capture. Finish/Back and idle remain reported; forged POST, missing-token and JavaScript-disabled checks remain pending. No duplicate screenshots or repeat old journeys are requested solely for this documentation update. See [the patron record](PATRON_HOLD_TEST_RECORD.md) and [image index](screenshots/README.md).

## Additional Sandesh evidence intake

Eight original captures from Sandesh's report are retained under screenshots/sandesh, alongside the earlier 28. This intake brought that historical checkpoint to 36. The later Harsimran and Windows captures bring the current total to 48. Actual final Windows CSV/PDF files are retained under test-evidence and reconciled separately from these component screenshots.
