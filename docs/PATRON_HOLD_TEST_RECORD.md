# Identity-linked patron holds — patch and acceptance record

Prepared against `107f643` on `feature/holds-notifications`, with acceptance reports through 15 October 2026, 09:30 Darwin and source/image verification following the 10:06 push report. This supplement follows the earlier checkpoint review. It adds a required patron placement path; it does not replace the existing staff/repair/transfer evidence or the unfinished team records. The addition integrates with Mehakpreet's Identity-linked kiosk and the shared circulation safeguards. Kamalpreet supplied a clean Windows build and package audit, reported PH01/PH02 outcomes and Finish/Back behaviour, and supplied four inspected PH01–PH03 captures, now present in pushed 6a69ade. The remaining security and seven-module checks are identified below.

## What changes

- A member selects **Join waiting list** on a borrowed/damaged catalogue or kiosk card, or from their private account. GET/POST `/Kiosk/Hold` requires login and an active verified Borrower link.
- The form accepts the item code and an active pickup branch. The server resolves membership from `UserManager.GetUserId(User)` and rechecks that link within the item transaction. It does not use a submitted membership number or borrower ID to choose ownership.
- `HoldPlacementService` is shared with staff `/Holds/Create`: borrowed/damaged eligibility, no active transit, no duplicate active request, no hold on the member's own active loan, active pickup branch, serializable transaction and the existing transaction-owned per-item lock.
- New requests are Waiting with server UTC creation time. Queue position uses the existing eligible FIFO order, with Id breaking timestamp ties. Joining does not generate a pickup notice; existing return/repair/transfer readiness does.
- Account and error views retain the member's own context. The patron form has antiforgery protection and the existing no-store/Finish/idle controls. Signed-in `/Kiosk` browsing now has Finish and the idle script, with the server inactivity check covering Index and Hold too.
- The feature changes no database model or migration. This evidence supplement adds two original PH03 photographs and updates documentation without replacing earlier screenshots or inventing hours, meetings or reflections. The deferred member-records patch remains separate.

## Current verification boundary

The build and runtime results in this record come from Kamalpreet's Windows project. Automated source/patch consistency checks are recorded separately. Kamalpreet supplied Windows build output at 08:26 Darwin on 15 October after build-server shutdown and generated obj/bin cleanup: compilation succeeded in 42.7 seconds, with two Pkcs package advisories repeated across restore/build. At 08:32 she supplied installation of System.Security.Cryptography.Pkcs 9.0.21, a successful build in 20.9 seconds without warnings, and an include-transitive NuGet audit reporting no vulnerable packages given the current sources. The build/audit outcomes are supported by the supplied terminal transcript; the security cases are listed individually below. Patron source/evidence commit 70db5b7 is now present in 6a69ade. The complete prepared combined tree matches the pushed files except the expected project package pin and two additional PH01/PH02 images; project package references match the fixed configuration and Pkcs 9.0.21 is verified. All 28 repository evidence images are present. This is source/evidence verification, not another Windows execution. The table distinguishes supplied transcript results, tester reports and inspected image observations. Cases without a supplied outcome remain pending; no comprehensive security or final seven-module acceptance is inferred.

| Check | Actual result | Status |
| --- | --- | --- |
| Build current combined Windows working source | Supplied clean build transcript on 15 October, 08:32 Darwin; 20.9 seconds, no warnings | Reported pass; source revision verified at 70db5b7 / 6a69ade |
| Fix reported Pkcs package advisories | 9.0.21 installed; include-transitive audit reports no vulnerable packages from current sources | Reported pass (transcript) |
| PH01 own request, pickup choice and no premature notice | At 08:49 Darwin on 15 October, Kamalpreet reported all guided outcomes matched: Sky / MBR-8001, TY-9901 Waiting position 1, Palmerston pickup and zero Ready-for-pickup records. One screenshot taken locally. | Account state observed in ph01_sky_waiting_palmerston.png; zero pickup notices remain reported |
| PH02 shared staff/patron duplicate guard and private progress | At 09:04 Darwin on 15 October, Kamalpreet reported all four guided outcomes matched: patron/staff duplicate rejection, Sky position 1 / Asha position 2 and only Sky's request in the private account. | Sky 1 / Asha 2 observed in ph02_shared_queue_sky_asha.png; duplicate actions and private-data check remain reported |
| PH03 unlinked/inactive login rejection | Images supplied at 09:15 Darwin on 15 October show Sky MBR-8001 inactive with hold access blocked, and a separate unlinked-login screen blocking placement. The 09:30 reply reports completion of the subsequent reactivation step. | Observed GET/UI blocking; reactivation reported; modifying-request checks remain separate |
| PH04 Finish/Back/idle and signed-in browsing | Idle notice/expiry reported at 09:08 Darwin. At 09:30 Kamalpreet replied Done after instructions to restore Sky, use Finish, require login on My Library Account and check Back cannot reopen a usable private account. | Finish/Back and idle reported; JavaScript-disabled server check pending; no comprehensive privacy pass inferred |
| PH05 forged identity input / missing antiforgery token | Not run | Pending |

Retain the build result and report exact outcomes. Do not mark a case passed from an expected-result paragraph. An unexpected result should be recorded and fixed before repeating only that affected case.

## One fresh test fixture

Do not alter TY-0001/TY-9001 or recreate their historical journey. Use a new copy **TY-9901**. If that code already exists, stop using it as a fresh fixture and choose another unused four-digit code consistently.

1. Run the HTTPS Development profile. Sign in as `admin@topendlibrary.local` / `Admin#2026!`.
2. Select **Front desk → Working location**: **Darwin Hub / DAR-D01**.
3. Open **Administration → Toys → Add toy** and enter:

| Field | Exact fictional input |
| --- | --- |
| Library Code | TY-9901 |
| Name | Wet Season Story Kit |
| Description | A reusable storytelling toy kit for families waiting through wet season repairs. |
| Toy type | Storytelling kit |
| Recommended age | 6-10 years |
| Manufacturer | Group 8 Demo |
| Material | Recycled plastic |

4. Save. New toys start **Available** at the selected Darwin branch. Edit this new toy, set **Status = Damaged**, and save; there should be no loan, transfer or old hold for this copy.
5. Use the Development-only verified patron **Sky Nguyen / MBR-8001**, login `patron@topendlibrary.local` / `Patron#2026!`. The seed must display Sky's linked membership. If it is unlinked or has a conflicting existing record, report it; do not link another member by guessing a card/email match.
6. Keep the staff window and an InPrivate/incognito patron window separate so their login cookies do not replace each other. Existing test member Asha is **MBR-9001**; confirm she exists and is active before the PH02 assisted request. Do not assume an entered card number creates a member.

## PH01 — member places their own request

In the patron window, browse `/Kiosk`, search **TY-9901**, and select its **Join waiting list** button. Sign in as Sky when asked. Check that the form identifies **Sky Nguyen / MBR-8001**, then choose **Palmerston Hub** as pickup branch and submit once. There is no field where the patron chooses another member.

Expected: the private account shows a success message and one request for TY-9901, **Waiting**, position **1**, pickup **Palmerston Hub**. In the staff Hold desk filtered to TY-9901, ownership is MBR-8001. The damaged item remains Damaged. In Notifications filter **TY-9901 / Ready for pickup / All channels**: **0** records. Waiting is not readiness.

Useful capture: `ph01_sky_waiting_palmerston.png`, showing the private account, item code, status, position and pickup branch. Report the zero-notice check in words unless it is unexpected.

**Reported result — 15 October, 08:49 Darwin:** Kamalpreet confirmed all results in the seven-step guided batch matched expectations and one screenshot was taken. That batch covered the new Damaged TY-9901 fixture, Sky's linked form and private Waiting position 1 at Palmerston, plus the zero pickup-only notification check. The screenshot is still local; its actual filename and image contents have not been verified. The staff-list ownership inspection listed elsewhere in this guide was not a separate step in that batch and is not inferred as independently observed. PH02 will verify the shared staff/patron entries and private projection. No actual task hours are inferred from elapsed message times.

## PH02 — the same queue across staff and patron entry

1. As Sky, use Join a waiting list again with **TY-9901 / Palmerston Hub**. Expected: duplicate-request error; still only one active Sky request. This verifies the new patron entry point.
2. In the staff window, use `/Holds/Create` with **TY-9901 / MBR-8001 / Palmerston Hub**. Expected: the same duplicate error, without another row. This verifies the shared staff service after its extraction.
3. Staff create **TY-9901 / MBR-9001 / Palmerston Hub** for Asha. Expected: Sky position 1, Asha position 2 in the staff list.
4. Refresh Sky's account in the patron window. Expected: only Sky's own TY-9901 request and position 1; no Asha name, card number, loan or fine is exposed in Sky's account. Staff records remain in the staff-only desk.

One combined staff queue capture `ph02_shared_queue_sky_asha.png` is useful. Report the duplicate and private-account checks; another screenshot is optional unless a failure needs diagnosis.

**Reported result — 15 October, 09:04 Darwin:** All four requested PH02 checks matched expectations according to Kamalpreet. The combined queue screenshot was requested; this reply did not separately confirm it was taken, and no image has been received for inspection. Kamalpreet also described repeated sign-in while navigating the member flow and then said she may have interpreted the screen incorrectly. At 09:08 she clarified that the inactivity timeout occurs and its message is shown. No immediate-login defect or comprehensive privacy pass is inferred. Subsequent membership and Finish/Back outcomes are recorded under PH03/PH04.


## PH03 — ownership and membership gate

In the staff window, visit `/Kiosk/Hold?libraryCode=TY-9901` using an existing staff login that has no Borrower Identity link. Expected: redirect to the account screen with the active-linked-membership message; no new hold. Sign in is necessary but does not grant another member's ownership.

For the inactive-member variant, only if Sky has **no active loan**, staff may edit MBR-8001, uncheck Active and save. Refresh the patron hold URL and account. Expected: inactive message and no creation form/new hold. Restore Active immediately after the check. Existing waiting history remains; do not cancel it to hide the result. If Sky has an active loan, do not force deactivation past the application's guard; record that this variant needs a suitable inactive fixture.

**Inspected images — 15 October, 09:15 Darwin:** [Inactive Sky](screenshots/ph03_sky_inactive_hold_blocked.jpg) shows the active-linked-membership placement warning, the explicit inactive message, Sky Nguyen / MBR-8001, no active loans and a retained Wet Season Story Kit request labelled Paused — ask staff. The item code is below the visible portion and is not claimed as readable in this photo. [Unlinked login](screenshots/ph03_unlinked_hold_blocked.jpg) shows the placement warning and account-not-linked message with no member details or hold form. The second photo does not show the signed-in email or role, so its admin identity is guided test context rather than image proof. Neither image shows Sky restored to Active. At 09:30 the lead replied Done to the subsequent three-step reactivation and Finish/Back batch; reactivation is therefore recorded as reported, without a new image. These captures do not constitute a forged POST or missing-antiforgery-token check.

## PH04 — leaving the shared kiosk

1. While Sky is signed in, open the new hold form, choose **Browse catalogue**, then **Finish and sign out** on `/Kiosk`. Open My Library Account again. Expected: login required.
2. Press Back after Finish. Expected: no usable prior private account; any restored page is refreshed by the existing pageshow handling. Report any visible private data or error page.
3. Sign in again, open Hold, return to `/Kiosk`, then leave it completely untouched for more than two minutes. Do not keep moving the pointer over the page. Expected: private access requires login, with the kiosk reset. Record the actual screen if expiry produces an error instead of a graceful return.
4. Repeat a private request after more than two minutes with JavaScript disabled, using a suitable signed-in test session. Expected: the server denies stale private access; the client script alone is not the enforcement boundary. Restore JavaScript afterwards.

No screenshot is mandatory for each step. Exact observed behaviour, elapsed idle duration and browser/window used matter more than repeating earlier hold captures.

**Timeout clarification — 15 October, 09:08 Darwin:** The lead reports that the two-minute inactivity behaviour occurs and the interface displays a two-minute-without-activity message. This observation is retained as reported idle-expiry evidence; elapsed duration was not independently measured and no new image was supplied. The earlier account-navigation concern is now explained by the lead as inactivity. A repeated idle wait or immediate-navigation diagnostic is not requested solely for that concern.

**Reported result — 15 October, 09:30 Darwin:** Kamalpreet replied Done to the three guided steps: restore Sky / MBR-8001 to Active; sign in as Sky in the private window, choose Finish and sign out, and confirm My Library Account requires login; then check Back cannot reopen a usable private account. This records the completion report for that batch. The reply did not specify which signed-in screen supplied the Finish button, independently timed expiry or give a new photograph; complete route-by-route cache behaviour is not inferred. The JavaScript-disabled stale-request check and PH05 POST checks remain pending. No additional screenshot is required for this reported batch.


## PH05 — focused form security checks

These are authorised checks on the local assessment app only. They deliberately POST a request, so use a fresh fixture **TY-9902**, created with the same fields and Damaged status as TY-9901. They are suitable for Mehakpreet's final security walkthrough rather than a prerequisite to applying the patch.

1. As Sky, open `/Kiosk/Hold?libraryCode=TY-9902`, select Palmerston, but do not press Submit. In browser Developer Tools → Console, run the following against that form:

```javascript
const holdForm = document.querySelector('form[action$="/Kiosk/Hold"]');
if (!holdForm) throw new Error('Open the signed-in hold form first.');
const request = new URLSearchParams(new FormData(holdForm));
request.set('MembershipNumber', 'MBR-9001');
request.set('BorrowerId', '9001');
request.set('MemberName', 'Asha Patel');
const response = await fetch(holdForm.action, {
  method: 'POST', credentials: 'same-origin', body: request
});
console.log(response.status, response.url);
```

The extra identity fields are deliberately untrusted. The posted BorrowerId value is arbitrary and is not Asha's confirmed database primary key. Expected: staff inspection shows the new request belongs to **Sky / MBR-8001**, with no forged owner, creation time or queue priority. Do not submit the normal form again; that would be a duplicate test rather than the first request.

2. Reload the signed-in hold form so it has a current token. Create the same request body, delete `__RequestVerificationToken`, and POST without a token header. Expected: HTTP **400** and no second request. If the login expired first, an authentication redirect is a different result; sign in again and repeat this specific check.

3. Signed out, open the Hold URL. Expected: login required and no private membership data. Forged query-string card/borrower values do not select an account.

## What this does not finish

The new placement path closes a source gap after acceptance, not the entire 60-mark assessment. Outstanding group work includes importer/report corrections and integration, remaining kiosk/API/circulation/notification acceptance, a successful clean-VM installation and missing team records. Contracts, actual or clearly labelled retrospective hours, real minutes, four individual reflections/value accounts and the final portfolio PDF remain required.

Technical references used during implementation: Microsoft [MVC model binding](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-10.0) and [MVC antiforgery protection](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-10.0). The controller uses a dedicated form model and trusted principal ownership; the tests above verify behaviour rather than inferring runtime success from these references.

## Pushed source and additional image inspection — 15 October

The lead reported the member-record push complete at 10:06 Darwin. Remote **6a69ade02b2f0a79b6beb035399eafd6c06dd731** contains **70db5b7**, the patron source/evidence/package update, and the member-record update. A fresh archive comparison verified every prepared combined file, with the expected project dependency change and the two additional images below. Pkcs is pinned to **9.0.21** and the full project package-reference set matches the fixed prepared configuration. The diff whitespace check passed. There are **28 evidence images**; no old image was replaced.

| Case | Inspected capture | Visible result and limit |
| --- | --- | --- |
| PH01 | [Sky's private request](screenshots/ph01_sky_waiting_palmerston.png) | Success message names MBR-8001 / TY-9901 / Palmerston Hub. Sky Nguyen has Active membership, no active loans, and one visible Wet Season Story Kit request: Waiting, position 1, Palmerston Hub. The zero pickup-notice check is reported separately. |
| PH02 | [Shared queue](screenshots/ph02_shared_queue_sky_asha.png) | Staff route filtered to TY-9901. Sky / MBR-8001 request #6, requested 08:46, is position 1; Asha / MBR-9001 request #7, requested 08:59, is position 2. Both are Waiting, Palmerston pickup and awaiting repair. The cards display newest first while the queue-position numbers preserve FIFO. The image does not show either duplicate submission or the private-account exposure check. |

PH01 was captured during placement, before the inactive-membership variant. Its Active label is not proof of later reactivation; reactivation remains the lead's 09:30 report. The two PH03 photographs retain the limits documented above. PH05 and JavaScript-disabled expiry remain pending; no complete privacy/security or final seven-module pass is inferred.

## Current completion actions

This record preserves its tested checkpoint and stated evidence limits. The latest member handoffs and integration/VM actions are in [REMAINING_WORK.md](REMAINING_WORK.md). Documentation updates do not require repeating earlier tests solely to rename or index their evidence.
