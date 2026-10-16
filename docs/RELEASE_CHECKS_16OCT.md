# Our combined release verification

Our verification combines Windows browser and SQL checks, Linux fixtures and the retained component evidence.

| Check | Result | Evidence basis |
| --- | --- | --- |
| Linux build/audit | Build passed; no vulnerable packages reported at the check. | Retained logs; SDK 10.0.402/runtime 10.0.13, EF 10.0.10. |
| Linux suite | 30 passed, 0 failed, 3 SQL skips. | Retained TRX/text. InMemory does not test SQL locking/foreign keys. |
| Windows build/SQL-enabled suite | 33 passed, 0 failed, 0 skipped at b79faf3. | Kamalpreet's Windows terminal transcript; TRX retained on the test machine. |
| Canonical migrations and circulation | Fresh isolated migration execution, rightful-owner collection, simultaneous checkout and late return passed. | Three real SQL cases. |
| Actual HTTPS API | All ten checks passed; key not printed. | Script passing output and explicit all-ten confirmation. |
| CSV browser | 3 copies/2 rows, repeat zero; mixed 1 accepted/2 skips; missing year header rejected. | Actual captures and inventory. |
| Manager CSV/PDF | Nine-item dataset reconciles to dashboard and audit rows. | Actual exported files inspected. |
| Non-zero payment | AUD 27 moves unpaid to paid; total/estimate/inventory unchanged. | Actual before/after CSVs. |
| Browser access/Finish | Signed-out challenge, Patron denial and fresh-request login after Finish. | Written Windows outcomes. |

Production tests cover fine/payment calculations, shared report eligibility, import validation/quantity, CSV formula escaping, PDF generation, API key/filters, role/antiforgery metadata and notification filter retention. A fixed clock crosses Darwin midnight. Synthetic exports use 13 items and AUD 9 fines; they are labelled separately from the actual nine-item Windows dataset and older 104/107-item member captures.

The separate VM installation failed at LocalDB setup. Runtime forged-POST, browser-history-cache and JavaScript-disabled expiry checks were outside the completed checks. Normal kiosk idle behaviour was observed; the source and metadata checks cover the implemented controls.

Detailed test results, transcripts, captures and actual export files are indexed in docs/test-evidence.
