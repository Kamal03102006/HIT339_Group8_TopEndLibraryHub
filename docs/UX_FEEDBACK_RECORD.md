# Our UX observations and improvements

We kept informal patron feedback separate from internal acceptance. Mehakpreet's record identifies Anjana, Robinpreet and **Navdeep**, confirmed through the member follow-up. No reviewer requested a specific further change; we do not attribute an invented issue to them.

| Observation | Change or response | Result/limit |
| --- | --- | --- |
| Lowercase membership input looked uppercase but failed validation. | Case-tolerant MBR regex with uppercase storage. | Source/regex checks verified; separate lowercase browser retest not recorded. |
| Damaged-copy checkout's generic message confused the test instruction. | Matched the expected result to the actual Borrow item screenshot. | Rejection visible; no damage-specific message change claimed. |
| Mixed notification types made pickup counts unclear. | Explicit item/type/channel filters and retention on immediate recheck. | Filtered total four confirmed; redirect regression passed. |
| Manager page had duplicate export-button pairs. | Removed the redundant pair in the final source. | Presentation-only cleanup; report calculations unchanged. |
| Shared kiosk needs clear next steps/privacy. | Collection, Self service, Join waiting list, My account and Finish, with Identity ownership. | Own-account/inactive/unlinked captures and fresh-request logout; no formal accessibility study claimed. |

Our green theme supports the practical flow. The main benefit is explaining repair, allocation, transit and readiness without treating an origin allocation as ready at the destination. We did not measure staff time saved or comparative novelty.
