# Kiosk and API integration handoff

Updated following review of pushed checkpoint a1e8190. Mehakpreet's 3b925ee source is integrated with the circulation work based on dbbc540. Kamalpreet's Windows build succeeded and all seven migrations show Applied=True. Final kiosk/private ownership/privacy/API and concurrent-checkout runtime results remain pending.

## Implemented in the prepared source

- Optional unique IdentityUserId ↔ Borrower link with restricted deletion; existing memberships remain unlinked.
- Account and checkout derive member ownership from the signed-in Identity user, with active/unlinked handling.
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

See [the integration test record](../docs/CIRCULATION_INTEGRATION_TEST_RECORD.md), especially C14–C19. Earlier component reports remain member-reported historical tests and do not verify this combined patch. Confirm the participant-name inconsistency in the feedback report before using it in the final portfolio.

Account summary fulfils the brief's checkout-or-summary option. The group proposes both, so shared checkout, privacy and API runtime checks remain required for the chosen scope.

## Follow-up from the whole-commit review

The brief's patron hold placement is not yet provided by this kiosk: current hold creation is staff-only. Coordinate a secure Identity-linked patron path. API branch/category-filter proposals are not current query parameters; resolve the contract. Verify the signed-in return to public kiosk browsing as part of Finish/Back/idle privacy testing, since the public index lacks the private views' Finish/idle controls. These are pending work, not claimed passes. See [the complete review](../docs/PROJECT_PROGRESS_REVIEW.md).
