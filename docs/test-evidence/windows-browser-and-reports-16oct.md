# Our actual Windows import and report evidence

I ran the browser checks and supplied the files as Kamalpreet. We inspected the captures, parsed the actual CSVs and rendered both pages of the actual PDF. These results follow the b79faf3 release import.

| Import | Actual result |
| --- | --- |
| books-valid.csv | 3 physical copies / 2 accepted rows / 0 skipped. |
| Repeat same file | 0 copies / 0 accepted / 2 skipped. |
| Mixed validation sample | 1 accepted row / 1 copy / 2 skipped; BK-0005 Another Valid Book visible. Lower error rows are outside the capture. |
| Missing year header | Missing columns: PublicationYear; no Missing Year Column item in actual inventory. |

The dashboard and before-payment CSV/PDF agree: 9 items, 4 generally available, 1 reserved Available, 0 in transit, 2 borrowed, 2 damaged, 0 destroyed; 7 loans, 2 active, 5 returned and 2 overdue. Before CSV generated 18:25:44 ACST; PDF 18:25:53. The nine-second generation-time difference is expected.

Loan 3 / TY-0001 / MBR-0001 was due 26 September and returned 14 October: 18 days at AUD 1.50 = AUD 27.00. The after CSV generated 18:37:46 records payment at 18:37:23.

| Metric, AUD | Before | After |
| --- | ---: | ---: |
| Recorded fines | 27.00 | 27.00 |
| Recorded paid | 0.00 | 27.00 |
| Recorded unpaid | 27.00 | 0.00 |
| Separate current late-charge estimate | 9.00 | 9.00 |

All inventory audit rows and loan/item counts stay unchanged. BK-0002 through BK-0005 are generally available. TY-9001 is physically Available but reserved; damaged TY-9901 with a pending request is unavailable but not a reserved Available copy. Both PDF pages were readable without observed clipping. A duplicate export-button pair in the older capture was removed from the final source without changing calculations. A repeat payment is tested automatically, not inferred as another browser run.
