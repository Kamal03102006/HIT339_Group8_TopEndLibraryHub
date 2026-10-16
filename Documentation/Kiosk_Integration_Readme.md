# Kiosk and secure catalogue API integration

Mehakpreet's kiosk/API contribution is combined with Kamalpreet's circulation/hold/privacy work and Harsimran's branches/transfers. The 16 October release also connects the API's availability predicate to manager reporting. Earlier test reports remain historical evidence; current verification and pending Windows checks are in [release checks](../docs/RELEASE_CHECKS_16OCT.md).

## Current behaviour

The public catalogue and kiosk can be browsed without login. Account, checkout and hold actions derive ownership from the authenticated Identity user through the optional unique Borrower.IdentityUserId link. Existing borrowers are not automatically linked by email. Inactive and unlinked accounts cannot borrow or place new requests. Staff-assisted and patron requests use HoldPlacementService; checkout uses CirculationService for active membership, current location, transfers, active loans and reservation ownership. The rightful Ready member can collect while later members wait.

The account displays only that patron's loans, requests, recorded unpaid fines and separately labelled current late-charge estimates. Private responses use no-store. Finish signs out; patron tickets and the kiosk inactivity check expire after two minutes, including on the browsing screen when signed in. The final JavaScript-disabled expiry and forged POST checks remain pending.

## Demonstration and configuration

Development seeds the fictional patron@topendlibrary.local / Patron#2026! account linked to Sky Nguyen, MBR-8001, without staff roles. Preserve existing memberships and conflicting codes. This account is a local demonstration; do not reassign an existing real member to it. A general staff verification/linking interface is outside the chosen scope.

Set Kiosk:BranchCode to DAR, PAL or CAS in user-secrets and restart. This trusted server setting identifies the physical station; posted branch IDs do not choose checkout location. Set CatalogueApi:ApiKey to a private random secret, following [installation instructions](../docs/INSTALLATION_GUIDE.md). Send X-API-Key over HTTPS. Unconfigured key returns 503; missing/wrong key returns 401.

## API contract

GET /api/catalogue searches by search and accepts limit=1..100, category=Book/Music/Toy and an active branchId. GET /api/catalogue/available uses the same filters and shared general availability. GET /api/catalogue/categories lists item categories; /status reports service/database health and configured active branch flags, not live opening hours; /{code} returns a catalogue item. Responses exclude patron fields. No write endpoints are provided.

## Evidence and remaining checks

The member's successful component checkout, account and missing-key capture are preserved in her report. Earlier lead hold/access photographs remain indexed. Current automated tests exercise production availability/filter/middleware and ownership-related circulation code; three real SQL Server circulation tests and final HTTPS/browser checks are separate. Run scripts/Test-Release.ps1 -IncludeSqlServer and scripts/Test-CatalogueApi.ps1 on the final Windows source, then record actual results. See [patron guide](PatronUserGuide.md) and [historical acceptance record](../docs/PATRON_HOLD_TEST_RECORD.md).
