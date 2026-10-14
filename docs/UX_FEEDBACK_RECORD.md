# UX feedback and improvement record

Use actual observations, implemented changes and retest outcomes. This record does not establish that the entire application has passed acceptance testing.

## UX-01: membership number capitalisation

- **Date:** 14 October 2026, Darwin time.
- **Tester:** Kamalpreet, testing the front-desk borrower registration workflow during group integration.
- **Task:** Register the first fictional hold-test borrower using membership number `MBR-9001`.
- **Observation:** The registration form displayed `MBR-9001` but showed the validation error "Use the format MBR-0001." The supplied screenshot is `image-1791964838891.jpg`.
- **User confirmation:** Kamalpreet confirmed that lowercase letters had been typed, although the field displayed capitals. Retyping the prefix in uppercase allowed registration to complete.
- **Verified source cause:** The input uses Bootstrap's `text-uppercase` display class. The Borrower model's original `^MBR-\d{4}$` validation pattern accepts only uppercase letters, and the browser's regex adapter checks the underlying value. The controller already converts a valid membership number to uppercase before storage.
- **Change implemented and reported:** Kamalpreet confirmed on 14 October at 17:50 that the Borrower validation pattern was changed to `^[Mm][Bb][Rr]-[0-9]{4}$`. This accepts uppercase, lowercase and mixed-case versions of the same prefix while retaining the hyphen and exactly four ASCII digits. Existing controller normalisation and duplicate checks remain in place.
- **Checks completed here:** Source inspection and local regex checks for the reported lowercase input, uppercase and mixed-case input, and malformed membership numbers. These are not Windows runtime checks.
- **Application status:** The pattern is verified in pushed commit dbbc540. Local regex checks passed; the specific Windows lowercase registration retest remains pending.
- **Retest:** Enter `mbr-9002` during borrower registration and confirm that the saved record is `MBR-9002`. The written record will be updated from the tester's result; Kamalpreet does not need to write it. Duplicate rejection with another letter case and malformed-input rejection remain separate checks.
- **Client value:** Staff can enter membership codes naturally without a mismatch between what the form displays and what it accepts.

| Retest | Expected outcome | Actual outcome/evidence |
| --- | --- | --- |
| Lowercase registration (`mbr-9002`) | Accepted; stored as `MBR-9002`. | Pending |
| Duplicate with another letter case | Existing membership is rejected rather than creating another borrower. | Pending |
| Malformed membership number | Missing hyphen or wrong digit count is rejected. | Pending |

Kamalpreet reports the observed behaviour; the written account, requirement links, change explanation and retest result are maintained from those reports. Five current hold-test screenshots are committed at dbbc540 with the tester's filenames. They verify the separate hold tests; they do not establish the lowercase registration retest.

## UX-02: damaged-checkout wording clarification

On 14 October at 20:58 Darwin, Kamalpreet questioned the general unavailable message during the guided damaged-copy checkout test because the guidance described rejection due to damage. Source review confirms the current checkout message is This item is not currently available for borrowing for non-Available stock. The user's last wording mentioned return, so the exact displayed message/page still needs confirmation. This is a reported clarity issue; no damage-specific wording change or retest has been claimed. The intended requirement is that damaged checkout is blocked until repair.

At 21:02 Darwin, the supplied c07_damaged_toy_checkout_rejected.jpg screenshot resolves UX-02's page/wording ambiguity: the page is Borrow item and the message ends with borrowing. No code correction is needed to satisfy the rejection check. The generic wording may still be clarified in a later UX improvement, but no such change is claimed here.

## UX-03: mixed notification counts during pickup verification

- **Date/tester:** 14 October, 22:27 Darwin; Kamalpreet during the TY-9001 repair/transfer/collection journey.
- **Observation:** The tester expected four pickup messages but reported six results: two Noah rows and four Asha rows, with Asha's rows including both Loan confirmed and pickup types.
- **Source check:** The GET action applies a distinct Kind filter; Ready for pickup is Kind=4 and Loan confirmed is Kind=1. The immediate CheckNow action currently redirects to Index without preserving filters. The actual selected filter and button sequence were not supplied, so the cause of this specific view is not treated as independently confirmed.
- **Action taken:** Testing guidance now explicitly selects Search TY-9001, Ready for pickup, All channels and clicks Filter records. It also reapplies filters after an immediate scan before comparing counts. Six mixed-type rows are kept distinct from four pickup-only rows in the written record.
- **Retest:** Pickup-only count four, followed by unchanged four after rechecking/reapplying filters. Actual outcomes pending.
- **Implementation status:** No filter-preservation code change has been made or claimed at this checkpoint.

At 22:33 Darwin, Kamalpreet replied **4** after the clarified pickup-only filter/recheck guidance. This is a reported matching filtered total. The effect of the clarification is supported by that report; exact screenshot contents, a separately enumerated repeat comparison and any future code change are not inferred.
