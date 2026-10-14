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
- **Application status:** The model edit is confirmed by the user. Local source and regex checks passed; the specific Windows lowercase registration outcome and subsequent pushed revision have not yet been supplied.
- **Retest:** Enter `mbr-9002` during borrower registration and confirm that the saved record is `MBR-9002`. The written record will be updated from the tester's result; Kamalpreet does not need to write it. Duplicate rejection with another letter case and malformed-input rejection remain separate checks.
- **Client value:** Staff can enter membership codes naturally without a mismatch between what the form displays and what it accepts.

| Retest | Expected outcome | Actual outcome/evidence |
| --- | --- | --- |
| Lowercase registration (`mbr-9002`) | Accepted; stored as `MBR-9002`. | Pending |
| Duplicate with another letter case | Existing membership is rejected rather than creating another borrower. | Pending |
| Malformed membership number | Missing hyphen or wrong digit count is rejected. | Pending |

Kamalpreet reports the observed behaviour; the written account, requirement links, change explanation and retest result are maintained from those reports. The three current hold-test screenshots are still on the laptop, with filenames and captions chosen by the tester. They are not yet included as repository evidence.
