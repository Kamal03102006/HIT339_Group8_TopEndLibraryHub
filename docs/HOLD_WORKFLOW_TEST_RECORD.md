# Holds desk test and evidence record

Updated 14 October 2026. Tester: Kamalpreet, using the Windows application. Dates and times are Darwin/ACST (UTC+09:30). The prepared source is based on merge `74668e1`; the tested desk, interface and pickup-branch migration have not yet been verified in a subsequent pushed commit.

Kamalpreet reported completing the three guided hold tests at 17:45 and confirmed at 17:51 that three screenshots are saved on the laptop. The individual outcomes have not yet been restated or checked against those images. The table therefore records completion as reported, with screenshot verification outstanding. Other acceptance checks remain pending.

## Implemented scope

The staff desk supports holds on borrowed or damaged physical items, active pickup branches, eligible waiting positions ordered by creation time then request ID, duplicate active-request protection, and cancellation of waiting requests with history retained. Admin and Reception can use the desk. The shared green interface includes navigation to holds, working location and transfers.

Automatic allocation, ready-for-pickup notifications, patron self-service placement and the loan, repair and transfer handover are the next integration stage.

## Migration review

Kamalpreet generated and supplied `LinkHoldsToPickupBranches` on 14 October. Its `Up` method was reviewed: it creates `IX_Holds_PickupBranchId` and adds `FK_Holds_Branches_PickupBranchId` from `Holds.PickupBranchId` to `Branches.Id` with restricted deletion. Its `Down` method removes the foreign key and then the index. No unrelated schema change appears in the supplied migration. The Windows application was subsequently used for the reported test batch; a separate database-update transcript has not been supplied.

## Guided sample data

These are the supplied test inputs. The actual chosen item and pickup branch will be matched to the screenshots when they are copied into the project.

| Record | Guided value |
| --- | --- |
| First fictional member | `MBR-9001`, Asha Patel |
| Second fictional member | `MBR-9002`, Noah Wilson |
| Item example | `TY-0001`, provided it is currently Borrowed or Damaged and has no existing active holds |
| Pickup branch example | Darwin Hub |
| Preparation | Register both members as active; neither should already have the selected item on loan |

## Reported test batch

| ID | Action | Expected outcome | Reported completion and evidence |
| --- | --- | --- | --- |
| H01 | Place the first member's hold, then submit the same member/item request again. | Duplicate rejected; one active request for that member/item remains. | Procedure reported completed on 14 October. One of the three screenshots is held locally; filename and content verification are pending. |
| H02 | Add the second eligible member's hold for the same item. | Original request has position 1 and the second position 2. Display order may be newest first. | Procedure reported completed on 14 October. One screenshot is held locally; the individual outcome and image are to be confirmed. |
| H03 | Cancel the first member's waiting request. | Cancelled history remains; the second eligible member moves to position 1. | Procedure reported completed on 14 October. One screenshot is held locally; the individual outcome and image are to be confirmed. |

No screenshot has yet been copied into `docs/screenshots` or supplied for these three results. The tester's existing filenames and captions can be retained. The documentation will link the actual filenames after they are available; expected outcomes above are not substituted for observed results.

## Issues found during testing

**Membership number capitalisation:** The borrower registration form displayed uppercase letters while its original validation required uppercase input. Kamalpreet reported that retyping the first membership number in uppercase allowed registration to complete. The model validation has now been changed to accept uppercase, lowercase and mixed-case prefixes; the controller still stores uppercase membership numbers. See [UX feedback record](UX_FEEDBACK_RECORD.md). The change was confirmed by the user at 17:50; the specific lowercase registration retest outcome remains to be confirmed.

**Missing second borrower:** During the second-member procedure, the holds desk reported that the supplied membership number was not found. Registration of the second fictional borrower was identified as the required setup step. A holds form references an existing borrower; it does not register a new borrower. The later overall test-completion report does not independently establish the exact borrower record or entered code.

## Remaining acceptance checks

| Check | Expected outcome | Current status |
| --- | --- | --- |
| Both borrowed and damaged item cases | Requests work for each permitted status. | Separate evidence for both statuses is pending. |
| Unknown, inactive or tampered inputs | Invalid item, borrower or pickup branch rejected without creating a hold. | Missing-member message reported; other cases pending. |
| Item already borrowed by requester | Request rejected with an explanation. | Pending. |
| Repeat cancellation | No second transition or timestamp rewrite. | Pending. |
| Staff authorization and privacy | Unauthorized users cannot access private hold data or submit changes. | Pending runtime check. |
| Inactive queue eligibility | Inactive member or pickup branch pauses eligibility while retaining the original timestamp. | Pending. |
| Return and repair allocation | The next eligible request is allocated when the item becomes usable. | Blocked by the next integration stage. |
| Pickup transfer and notification | Reservation remains protected in transit; only the correct member is notified after arrival. | Blocked by the next integration stage. |
| Shared checkout restrictions | Reception, kiosk and API apply consistent hold, loan and transfer rules. | Blocked by shared circulation integration. |

## Record maintenance

Written results are compiled from Kamalpreet's reported observations and supplied screenshots. Kamalpreet does not need to author the narrative or tables. Individual results, the tested pushed revision and image references will be added as they are reported. Real working hours are recorded separately in the group work record.
