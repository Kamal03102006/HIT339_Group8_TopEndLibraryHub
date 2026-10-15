# Kiosk and API integration handoff

Updated through the patron-hold acceptance reports of 15 October, 09:30 Darwin, following pushed checkpoint 107f643. Mehakpreet's 3b925ee source is integrated with the circulation work based on dbbc540. All seven migrations were reported Applied=True. The subsequent patron source has a supplied clean Windows build and package audit; its own-request, duplicate/FIFO/private-progress and Finish/Back checks are reported, with two inspected inactive/unlinked access photographs. Final kiosk checkout/API/security and concurrent-checkout acceptance remain pending; the current tested source still needs its pushed revision identified.

## Implemented in the prepared source

- Optional unique IdentityUserId ↔ Borrower link with restricted deletion; existing memberships remain unlinked.
- Account and checkout derive member ownership from the signed-in Identity user, with active/unlinked handling.
- GET/POST `/Kiosk/Hold` now allows an active linked patron to place their own waiting request. The form binds item code and pickup branch only; membership display fields are not trusted inputs. Staff and patron creation both use `HoldPlacementService` and the existing transaction-owned item lock, active-request index and UTC FIFO order. No new migration is required.
- Staff and kiosk checkout use CirculationService for location, active loan, transit and pickup-allocation checks within an item transaction.
- Public catalogue, kiosk and secure read-only API share general availability; a reserved physical Available copy is shown On hold and excluded from ordinary availability.
- Account shows only the patron's loans, recorded unpaid fines, separately labelled current-loan estimates and active hold progress.
- Private kiosk screens use no-store, Finish/sign-out, a two-minute inactivity check and screen reset. Patron authentication tickets expire after two minutes; staff sessions retain their working-location duration.
- GET API endpoints: /api/catalogue, /available, /categories, /status and /{code}. /status reports service/database health, not opening hours.

The earlier SharedBorrowingRulesIntegrated switch and blocked checkout stub have been removed. The combined application has been built, but a kiosk runtime checkout pass has not yet been supplied.

## Database update

`20261011090000_LinkPatronIdentity` adds the optional identity column/FK/index and filtered unique active-loan index. Other group migrations and pickup FK are retained. Inspect any migration error; do not drop the database or remove borrowing history to resolve it. Existing duplicate active loans must be resolved with staff before applying the unique index.

## Development demonstration patron

Startup in Development adds a separate fictional account and membership:

| Login | Local assessment password | Membership |
| --- | --- | --- |
| patron@topendlibrary.local | Patron#2026! | MBR-8001, Sky Nguyen |

The patron has no staff role. The seed preserves existing memberships, links and conflicting demo codes. It does not link Asha/Noah or grant ownership from card/email input. These are local fictional assessment credentials. A general staff identity-verification/linking UI and email confirmation delivery are outside this patch.

## Configuration

The kiosk branch is trusted configuration, default DAR. To test it at Palmerston, set Kiosk:BranchCode to PAL and restart; patrons cannot select an arbitrary checkout branch in the form.

Use user secrets for the API key; create your own random value and do not commit it:

```powershell
dotnet user-secrets set "CatalogueApi:ApiKey" "<your-long-random-key>"
dotnet user-secrets set "Kiosk:BranchCode" "DAR"
```

Send the key in the X-API-Key header over HTTPS. Missing configuration returns 503; missing/wrong credentials return 401.

## Verification

See [the integration test record](../docs/CIRCULATION_INTEGRATION_TEST_RECORD.md), especially C14–C19, and [the new patron hold checks](../docs/PATRON_HOLD_TEST_RECORD.md). Earlier component reports remain member-reported historical tests and do not verify this combined patch. Confirm the participant-name inconsistency in the feedback report before using it in the final portfolio.

Account summary fulfils the brief's checkout-or-summary option. The group proposes both, so shared checkout, privacy and API runtime checks remain required for the chosen scope.

## Follow-up from the whole-commit review

The patron hold patch addresses the earlier source gap with an Identity-linked placement form and shared staff/patron validation. The signed-in public kiosk index now includes Finish and the idle script, and the server inactivity check covers Index/Hold as well as the earlier private actions. The 15 October build transcript reports success without warnings after System.Security.Cryptography.Pkcs 9.0.21 was installed, and its include-transitive audit found no vulnerable packages given current sources. PH01/PH02 own-request, pickup, shared duplicates, FIFO and private-account checks are reported; PH03 photographs show GET/UI blocking for inactive/unlinked memberships; PH04 Finish/Back and idle behaviour are reported. JavaScript-disabled server expiry, forged identity input and missing-token checks remain pending for the final walkthrough. None of these results establishes full kiosk checkout or API acceptance. API branch/category-filter proposals are not current query parameters; resolve the contract. See [the earlier checkpoint review](../docs/PROJECT_PROGRESS_REVIEW.md) and [the current patch test record](../docs/PATRON_HOLD_TEST_RECORD.md).
