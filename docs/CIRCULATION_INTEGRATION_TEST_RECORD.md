# Circulation integration test record

Updated with the screenshot audit and whole-commit review of pushed a1e8190. The application source matches the applied circulation patch against dbbc540, including Mehakpreet's 3b925ee contribution. Windows build and all seven applied migrations are confirmed. The current table below includes inspected images; dated narratives preserve the reports as received. Source/patch checks are separate from Kamalpreet's Windows tests.

## Current result summary

| Check | Actual result | Evidence status |
| --- | --- | --- |
| Setup | Build succeeded; all seven migrations Applied=True. | User report and supplied console output |
| C01 | Noah Ready at Darwin after the guided usable return of TY-0001. | Supplied screenshot inspected |
| C02/C03 | TY-0001 pickup count stayed 2 → 2 after rechecking. | Count reported; inspected image now confirms Noah Email/SMS, Darwin and the pickup message. |
| C04–C06 | Asha blocked from Noah's reservation; Noah collected; repeat checkout rejected with one active loan. | Guard/count results reported; Noah Collected card inspected. |
| C07 | Damaged-copy TY-9001 checkout rejected; initial Asha/Noah queue awaits repair. | Rejection photograph and initial FIFO/Awaiting repair images inspected. |
| C08 | Repair allocated Asha for Palmerston; no early pickup records. | Allocation/Noah Waiting card and zero filtered records inspected. |
| C09/C10 | Correct dispatch succeeded; wrong destination, transit checkout and origin receipt rejected. | Dispatch/receipt error and transit row images inspected; transit borrowing rejection remains reported, with its form labelled input-only. |
| C11 | First Palmerston receipt produced Asha Ready and two pickup records. | Received transfer row, Asha Ready/Noah Waiting and Asha Email/SMS naming Palmerston inspected. Repeat receipt pending. |
| C12 | Asha Collected, Noah Ready; pickup-only total four. | Collection/next-family cards inspected; four pickup total reported. Exact loan/fine details remain separate. |
| C13 | A new damaged return left Asha Waiting and four pickup records; recorded repair increased pickup-only total to six. | Later results reported; supplied Asha #5 image is setup/Awaiting return, not the after-damaged-return state. |
| C19 pickup history | Pickup-only count stayed 6 → 6 across stop/start on the same Darwin date. | Reported; no new screenshot requested |

Kiosk/API/private ownership, simultaneous checkout, remaining loan/fine notification cases, repeat receipt and inactive eligibility remain separate checks. The historical C12 wrong-member Noah attempt was not explicitly restated; the shared reservation guard has the earlier C04 report. A fresh-copy procedure is in [the circulation demo guide](CIRCULATION_DEMO_GUIDE.md). The narrative below preserves the received reports and their limits.

## Windows setup evidence received

- At 19:55 Darwin, Kamalpreet reported the build completed after applying the 54-file patch.
- The Package Manager Console transcript received at 20:02 contains Build succeeded, and its latest Update-Database ends with No migrations were applied. The database is already up to date, followed by Done.
- Earlier commands in the same transcript applied branch/desks, transfers and the pickup-branch FK. The initial transcript did not show LinkPatronIdentity application. The subsequent Get-Migration output supplied at 20:06 Darwin resolves the check: all seven listed migrations, including LinkPatronIdentity and LinkHoldsToPickupBranches, show Applied=True.
- Build succeeded is visible in the migration-list output. This verifies the selected project/context migration state, including the new identity link and active-loan index migration. The later Ready screenshot confirms the application was running. The workflow evidence below distinguishes screenshot observations from user reports; concurrency and clean-VM installation remain pending.

## Observed application result — C01 ready allocation

The screenshot supplied at 20:15 Darwin, c01_noah_ready_darwin.png, shows Noah / MBR-9002 / request #2 for TY-0001 / Build the Top End as **Ready for pickup**, pickup Darwin Hub. It was supplied after the guided usable-return procedure. This verifies the displayed allocation result compared with the earlier Waiting position 1 evidence. The screenshot does not show the return form, loan return timestamp, pickup notifications or the lower Asha card; those details are not independently inferred. Later count, checkout, repair and dispatch results are recorded separately below.

Evidence: [c01_noah_ready_darwin.png](<screenshots/c01_noah_ready_darwin.png>).

## First check: the actual existing queue

At the start of this test sequence, the five verified screenshots left TY-0001 / Build the Top End awaiting return, Asha (MBR-9001) Cancelled, Noah (MBR-9002) Waiting at position 1, pickup Darwin Hub. The sequence used these existing records without recreating requests or deleting history. The later C04–C06 report records Noah's completed collection; this setup describes the starting state.

| Step | Exact action | Expected outcome |
| --- | --- | --- |
| C01 | Sign in as Admin. Front desk → Working location: Darwin Hub / DAR-D01. Return TY-0001 using today's displayed date. Leave “Item needs repair” unchecked. | One active loan is returned. Noah becomes Ready for collection at Darwin Hub. |
| C02 | Open Holds, search TY-0001. Open Notifications, filter TY-0001, Ready for pickup, All channels. If needed run the immediate check once. | Noah's hold is Ready; one pickup Email and one SMS identify Noah, TY-0001 and Darwin Hub. |
| C03 | Run the same notification check again. | No duplicate pickup event/channel is added. |
| C04 | Front desk → Borrow item: TY-0001, MBR-9001, due date 14 days after the displayed current date. | Rejected because the copy is reserved for the next member. |
| C05 | Repeat Borrow item with TY-0001, MBR-9002, the same future due date. | Checkout succeeds. Exactly one active loan belongs to Noah; hold #2 becomes Fulfilled. |
| C06 | Submit another checkout for the now-borrowed copy. | Rejected; no second active loan. |

Capture the ready hold + pickup messages, wrong-member rejection and correct-member loan/fulfilled hold. Preserve the earlier five screenshots. Report the actual messages; documentation is maintained from those reports.

## Reported pickup recheck result — C02/C03

Kamalpreet confirmed at 20:30 Darwin that the filtered TY-0001 / Ready for pickup / All channels result stayed at **two records** after using Check loans and pickups now. This is a **reported pass for the unchanged repeated-check count (2 → 2)**. No pickup-log screenshot has been supplied yet, so exact message wording, recipients, channel pairing and event timestamps are not independently image-verified. It is not evidence of overlapping scans, restart behaviour or another business date.

## Reported reservation and checkout results — C04–C06

At 20:40 Darwin, Kamalpreet confirmed that all three results matched the guided expectations. These are **tester-reported passes**, with screenshots for these outcomes not yet supplied.

| Check | Reported outcome | Evidence limit |
| --- | --- | --- |
| C04 | Asha MBR-9001 was rejected while TY-0001 was reserved for Noah. | Exact rejection screenshot pending. |
| C05 | Noah MBR-9002 borrowed the toy successfully; his hold showed Collected. | Successful loan/Collected screenshot pending. |
| C06 | Repeated checkout was rejected; Active TY-0001 filter showed one loan, belonging to Noah. | One-active-loan screenshot or query output pending. |

No simultaneous reception/kiosk test has been run by this sequence. Subsequent repair/dispatch results and the remaining checks are recorded below.

## Separate repair and transfer journey

Use a new fictional toy TY-9001 at Darwin, rather than reusing the completed queue above. Admin stock creation uses the selected working desk and starts Available. Edit its status to Damaged. Create fresh holds for Asha then Noah, both pickup Palmerston Hub.

| Step | Action | Expected outcome |
| --- | --- | --- |
| C07 | Attempt a hold on the damaged copy and checkout while Damaged. | Holds accepted; checkout rejected. No pickup notice before repair. |
| C08 | Admin edits TY-9001 to Available after its repair. | Asha is Allocated; pickup Palmerston. The copy is absent from generally available catalogue/API results. |
| C09 | At DAR-D01, try dispatch to Casuarina, then dispatch to Palmerston. | Wrong destination rejected. Correct dispatch creates one InTransit transfer, preserving Asha's allocation. |
| C10 | Attempt reception/kiosk checkout in transit, and receipt at Darwin. | All rejected; no loan or early ready notification. |
| C11 | Select Palmerston / PAL-D01 and receive the transfer once, then repeat receipt. | First receipt moves the physical copy and makes Asha Ready. Repeat changes nothing. Only Asha receives pickup simulations. |
| C12 | Try Noah checkout first, then Asha checkout; return Asha's loan at Palmerston in usable condition. | Noah initially blocked; Asha succeeds and fulfils her hold. Return makes Noah Ready and notifies him. |
| C13 | Repeat a return but mark “Item needs repair” on a genuinely new active loan. | Returned copy is Damaged; waiting requests remain waiting until repair. |

Do not mark these checks passed from the expected-outcome column.

## Damaged-copy checkout evidence — C07

At 20:58 Darwin, Kamalpreet reported completing the TY-9001 damaged-copy setup and asked about the general rejection message. The screenshot supplied at 21:02 resolves the wording/page check: it shows **Borrow item**, Library Code **TY-9001**, Membership **MBR-9001**, due **28/10/2026**, and **This item is not currently available for borrowing.** This is an **observed pass for the borrowing rejection UI** in the guided damaged-copy scenario. The photograph does not independently show the item condition, both waiting requests or a database loan count; those setup details are tester-reported. Subsequent repair, dispatch and first destination-receipt reports are recorded below.

Evidence filename: c07_damaged_toy_checkout_rejected.jpg. The current message is shared by non-Available stock and an existing active loan; it is not a damage-specific explanation.

## Reported repair allocation and no early notice — C08

At 21:17 Darwin, Kamalpreet reported that the guided repair of TY-9001 produced **Allocated for Asha** and the filtered pickup-notification result showed **zero records**. These are reported passes for allocation and absence of an early pickup notice. The user has captured the allocated-Asha screenshot locally but has not supplied it here. The recommended zero-result capture was reported complete at 21:21; it provides a before/after record for destination receipt once supplied. This is an evidence recommendation, not a quoted rubric requirement for every test screenshot. Noah's remaining waiting state and catalogue/API availability exclusion have not been separately verified in this report.

## Reported transfer dispatch and rejection checks — C09 / reception portion of C10

In the subsequent 14 October evening update, Kamalpreet reported completing the latest transfer batch and obtaining the expected results. These are **reported passes**, not independently inspected screenshots or SQL results.

| Check | Guided inputs | Reported outcome |
| --- | --- | --- |
| C09 / wrong destination | Working Darwin Hub / DAR-D01; TY-9001; destination Casuarina Hub while Asha's pickup is Palmerston. | Dispatch rejected. |
| C09 / correct dispatch | Same item/location; destination Palmerston Hub. | Transfer created with origin Darwin, destination Palmerston and In transit status. |
| C10 / reception checkout during transit | Borrow TY-9001 / MBR-9001; due 28 October 2026. | Checkout rejected pending destination receipt. |
| C10 / receipt at origin | Receive TY-9001 while working at Darwin / DAR-D01. | Receipt rejected; transfer remains In transit. |

For this batch, retain two useful evidence screenshots: the wrong-destination rejection and the wrong-branch receipt rejection (with the transfer row if it fits). Additional captures may be kept, but the record does not require an image of every click. Filenames and images have not yet been supplied here. The dispatch reports alone do not establish kiosk transit rejection, successful destination receipt, subsequent pickup notices, repeated receipt or simultaneous checkout. The later first destination-receipt result is recorded next.

## Reported first destination receipt and pickup records — C11, partial

At 21:49 Darwin on 14 October, Kamalpreet confirmed the latest guided Palmerston receipt step produced the expected result and explicitly said no screenshot was taken. The guided inputs were Working location Palmerston Hub / PAL-D01 and receipt of TY-9001. The expected outcomes confirmed by that report were **Asha Ready for pickup** and **two filtered pickup simulations, one Email and one SMS**. This is a **reported pass for first receipt and those displayed outcomes**; it is not an image-inspected result.

Exact message wording/recipients, Noah's waiting state, the transfer row/physical-branch display, repeated receipt and simultaneous operations have not been independently verified by this report. Save the current Asha Ready card and the two filtered pickup rows before the next checkout. Re-run the manual notification check with TY-9001 / Ready for pickup / All channels; the count should stay two, but its actual recheck result is still pending. The C12 wrong-member/right-member collection and next-family handover remain pending.

At 21:54 Darwin, Kamalpreet reported taking the notification and transfer/transit captures locally and asked whether the Holds capture was also needed. The Asha Ready / Palmerston card remains the recommended remaining capture. Neither image has been supplied here, and this capture report does not establish the repeated-scan count.

At 21:57 Darwin, the tester acknowledged completion of the subsequent Holds capture/recheck instructions. The Holds capture is recorded as locally completed pending its filename/image. The actual numeric recheck count has not been restated, so the Palmerston repeat-count outcome is not inferred from Done.

## Next guided batch — C12 collection and next-family handover

Remain at Palmerston Hub / PAL-D01. Record the pre-checkout filtered pickup count before creating new pickup events.

| Action | Exact guided inputs | Expected result — not yet reported |
| --- | --- | --- |
| Attempt checkout for the later member | Borrow TY-9001 / MBR-9002 / due 28 October 2026. | Noah is rejected because Asha has the next reservation. |
| Checkout for the allocated member | Borrow TY-9001 / MBR-9001 / same due date. | Asha borrows successfully; her hold displays Collected. |
| Return the usable copy | Return TY-9001 / return date 14 October 2026 / NeedsRepair unchecked / PayFineNow unchecked. | Asha's loan closes with zero late fine; Noah becomes Ready for pickup at Palmerston. |
| Review next-family pickup history | Notifications TY-9001 / Ready for pickup / All channels. | Four pickup records in total: two earlier Asha records and two new Noah records, with one Email and one SMS for each. Borrowing-confirmation records are excluded by this message-type filter. |

Capture the resulting Holds cards for Asha Collected and Noah Ready, splitting the capture if necessary. The subsequent report below supplies only the outcomes it explicitly describes; other checks in this procedure remain pending.

## C12 reported collection/handover and notification count clarification

At 22:27 Darwin, Kamalpreet reported taking two screenshots: Asha Collected and Noah Ready. This records **reported displayed collection/next-member readiness outcomes** and **local screenshot capture completion**. The image filenames/content have not been supplied here. The prior Noah wrong-member rejection, zero fine, loan closure details and exact form dates were not explicitly restated in this message.

The tester reported **six notification results: two for Noah and four for Asha**, with Asha's rows including Loan confirmed and pickup messages. This is not a report of six pickup messages. The expected explanation for this mixed-type view is two earlier Asha pickup records, two Asha borrowing-confirmation records and two new Noah pickup records. Exact individual channels and applied filters have not been image-verified. The source maps Loan confirmed to Kind=1 and Ready for pickup to Kind=4; the GET filter restricts Kind when selected. The current CheckNow action redirects to the unfiltered Index, so filters must be reapplied after using the immediate check button. This is a possible contributor to the reported count confusion, not an observed interaction sequence.

Next: set Search TY-9001, Message type Ready for pickup, Channel All channels, then click Filter records. Expected pickup-only count is four, two for each member. After an immediate recheck, reapply these filters before comparing counts; expected four remains four. These actual filtered/recheck counts are still pending. The two reported Holds captures are sufficient for the selected collection/handover screenshots; no additional notification capture is required by this step's evidence selection.

At 22:33 Darwin, the tester replied **4** to the filtered-count/recheck guidance. Record **reported pickup-only count four**, matching the expected number after the next-member handover. The reply does not separately enumerate before/after values or restate the prior Noah wrong-member rejection. Exact channel/message content, repeated receipt, overlapping scans and restart remain separate evidence checks.

## Next guided batch — C13 return requiring repair

Remain at Palmerston Hub / PAL-D01 and use the existing TY-9001 / Noah Ready state. These steps are expected outcomes, not reported passes.

1. Borrow TY-9001 for Noah / MBR-9002, due 28 October 2026. Expected: successful loan and Noah Collected.
2. Create a new hold for Asha / MBR-9001 on TY-9001, pickup Palmerston Hub. Expected: a new Waiting request while Noah has the copy; the earlier Collected request remains history.
3. Return TY-9001 using the displayed current Darwin date (14 October for this evening's test), **NeedsRepair checked**, **PayFineNow unchecked**. Expected: loan returned, copy Damaged and Asha's new request still Waiting / Awaiting repair.
4. Filter notifications TY-9001 / Ready for pickup / All channels. Expected: four historical pickup records remain; no new ready notice is added for the new Asha request while the copy is damaged. Noah's new loan-confirmation rows are excluded by this message-type filter.

Capture the new Asha Waiting / Awaiting repair card after the damaged return. The current inventory status and numeric pickup count should also be reported. This sequence must not be marked passed until actual outcomes are supplied.

## C13 reported damaged return and waiting request

At 22:45 Darwin, Kamalpreet confirmed the guided batch produced the expected results. This records **reported passes** for Noah's successful checkout, Asha's new Waiting request, return with NeedsRepair selected leaving the copy Damaged and the new request Waiting, and the pickup-only total remaining four with no new ready notice for that request. Fine amount, database transaction atomicity and exact event/channel rows are not independently inferred.

The tester explicitly reported taking **one screenshot showing Waiting only, nothing else**. It is retained as reported capture of the waiting state; its filename and image have not been supplied. Do not caption it as proof of the item's Damaged status, the repair explanation or the notification count. Those outcomes are supported by the user's test report at this checkpoint.

Next planned continuation, not yet run: edit TY-9001 from Damaged to Available after the fictional repair while it is already at Palmerston. Asha's new request should become Ready immediately and two new pickup simulations should bring TY-9001 / Ready for pickup / All channels to six historical records. Restarting and reapplying the same pickup filters should retain six records without adding duplicates. This restart procedure covers pickup history only; the remaining loan/privacy/concurrency checks are separate.

At 22:50 Darwin, the lead questioned why the guidance repeated similar actions. The planned extra Noah wrong-member attempt was removed from this continuation: the shared reserved-member guard already has the earlier reported C04 rejection, so another attempt is not needed solely to repeat that guard. The unreported historical C12 Noah attempt is not retroactively marked passed. Retain the distinct normal-return, damaged-return, other-branch receipt and process-restart checks, explain their purposes, and avoid duplicating unchanged scenarios without a new failure or concern. The remaining repair/restart continuation has not yet been run.

At 23:02 Darwin, the tester reported that after returning the toy the new hold still showed Waiting and only four pickup notifications were present. This matches the earlier damaged-return state if the item still has Damaged status. The tester has not confirmed a successfully saved edit to Available, so the next repair/Ready/six-record outcome remains pending; no application defect or successful restart check is inferred. Clarified action: Administration → Manage toys → Edit TY-9001 → Status Available → save, then verify the saved list status before checking the new Asha request and pickup filter. If Available is confirmed but Waiting persists, obtain the exact waiting explanation/current branch and diagnose that observed state.

At 23:07 Darwin, after the clarified repair-edit instruction, Kamalpreet reported **It's six now**. This records a **reported pickup-only total of six following the completed-repair guidance**, compared with the prior four while the new request waited for repair. The exact saved toy-status display and Asha Ready card were not separately restated or image-inspected in this reply. No code fix is inferred; recording the repair resolved the reported count discrepancy. The process-restart check remains pending.

Next startup/recovery procedure: stop the app in Visual Studio with Shift+F5, start it again with the green https Run button, allow it to run for about one minute, then open Notifications and apply TY-9001 / Ready for pickup / All channels using Filter records. Expected result: six historical pickup records remain, with no duplicate generation added. Report the actual count; this is the final check in the current hold/repair/transfer batch, not completion of all seven modules or all recovery/privacy tests.

## Reported pickup-history restart result — C19, partial

At 23:15 Darwin, Kamalpreet replied **6** after the guided stop/start and filtered-count procedure. Compared with the six reported before restart at 23:07, this is a **reported pass for pickup history/count stability: 6 → 6**, on the same Darwin date. No new screenshot was requested for this check. Exact per-row event/channel equality, loan-reminder recovery, concurrency and a clean VM run are not established by this count alone. The current hold/repair/transfer batch is complete as reported, with the detailed image evidence still to be matched to the locally saved captures.

## Kiosk, API and concurrent checks

In Development, startup creates the separate fictional linked patron Sky Nguyen / MBR-8001 / patron@topendlibrary.local (demo password in the handoff). Existing Asha/Noah memberships are not automatically linked or claimed by email. Kiosk physical branch defaults to DAR and is a trusted configuration setting.

| Check | Procedure | Expected outcome | Status |
| --- | --- | --- | --- |
| C14 / private ownership | Patron login → Account. Compare with a distinct unlinked login; edit Sky's borrower contact via staff and return to Account. | Only linked member data; unlinked account has no loan data; staff edit preserves Identity link. | Pending |
| C15 / shared kiosk checkout | Patron scans a usable unreserved copy at DAR, borrows, then scans again. Also test a reserved/in-transit copy. | First valid loan succeeds; repeats/reservations/transit are blocked; account shows the loan. | Pending |
| C16 / privacy | Finish session, use Back, request Account again. Separately leave a private kiosk screen idle for >2 minutes, including a server request with JS disabled. | Private screen clears/signs out; cached private pages reload; next private request requires sign-in. | Pending |
| C17 / secure API | Configure a fresh API key outside Git. Test missing/wrong/valid key; GET catalogue, available, categories, status and code lookup. | 401 without correct key (503 if unconfigured); read-only projected data; reserved/transit items excluded from available. | Pending |
| C18 / simultaneous checkout | Two browser sessions submit reception and linked-kiosk checkout for the same unreserved copy. | Exactly one succeeds and one active loan exists; the other receives a safe retry/unavailable result. | Pending |
| C19 / recovery | Restart with unchanged ready/active records and rerun scans within the same Darwin date. | Durable history preserved; no duplicate event/channel or loan. | Pickup-only 6 → 6 reported; remaining loan/exact-row recovery checks pending |

The API's /status is **service/database health**, not branch opening hours. Branch opening-hour semantics remain a requirements question. The initial implementation uses staff-assisted hold placement and patron read-only hold progress; patron hold creation/cancellation and pickup expiry are not implemented.

The supplied zero-result and transfer captures are now matched in the screenshot index. Keep the distinction between reported outcomes and visible image evidence. [The whole-commit review](PROJECT_PROGRESS_REVIEW.md) also identifies secure patron hold placement as a remaining requirement gap; read-only kiosk hold progress is not a placement form.


## Screenshot inspection after pushed checkpoint a1e8190

All 21 pushed image files have now been inspected or matched to previously inspected bytes and are indexed in [screenshots/README.md](screenshots/README.md). Earlier dated notes that images were local or awaiting filenames are historical. The audit upgrades only outcomes actually visible: damaged FIFO, Asha allocation, zero early pickup records, wrong-destination/origin-receipt errors, first receipt, correct pickup channels and collection/next-member cards. The transit checkout form contains no visible rejection, and the new Asha #5 Waiting capture says Awaiting return. The later NeedsRepair-return and repaired/restart counts remain tester reports. See [the audit](SCREENSHOT_AUDIT.md) for scope and gaps.

## Current completion actions

This record preserves its tested checkpoint and stated evidence limits. The latest member handoffs and integration/VM actions are in [REMAINING_WORK.md](REMAINING_WORK.md). Documentation updates do not require repeating earlier tests solely to rename or index their evidence.
