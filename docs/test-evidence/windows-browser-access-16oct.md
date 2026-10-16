# My Windows browser access result

Tester: Kamalpreet, S389008. The table records the selected Windows browser outcomes.

| Check | Actual outcome |
| --- | --- |
| Signed-out /Manager and /CsvImport | Login required. |
| Patron /Manager and /CsvImport | Access denied. |
| Private account | Initially matched expectations; a later unlinked-account warning left that browser/account context uncertain. Earlier Sky captures remain separate ownership evidence. |
| Finish, then fresh /Kiosk/Account request | Asked for login, confirmed at 19:08 Darwin. |

The logout result is based on a fresh request that required login. Forged POST, browser-history-cache and JavaScript-disabled expiry were outside these browser checks.
