# Top End Library Hub Individual Report

Kamalpreet  |  S389008  |  HIT339 Distributed Development  |  Group 8

Contribution record as at 15 October 2026. Role: group lead, holds and simulated notifications, with shared integration and acceptance testing.

My contribution focused on keeping a patron's request connected to the condition and physical location of a library item. I worked on holds and simulated notifications, applied and checked shared changes in Visual Studio, and coordinated the evidence needed to show how the separate modules work together. The group used my Assignment 2 application as its starting point.

## 1 Contribution and task allocation

The shared work has named owners. Harsimran developed branches, desks and transfers; Mehakpreet developed the kiosk and API; Sandesh developed CSV import and manager exports. My integration work connects those contributions and keeps their authorship visible in the group record.

The documentation checkpoint is 9b28459 on feature/holds-notifications. Source milestones include 74668e1, dbbc540, a1e8190 and 70db5b7. Final integration of the newer member changes remains a group task.

## 2 Testing and evidence

I ran the guided checks in the Windows application and reported the actual messages, counts and status changes. The evidence records distinguish what is visible in a screenshot from an outcome I reported after testing. The following examples show the coverage of my work.

## Acceptance work still open

The recorded results establish selected local workflows. Simultaneous checkout, focused forged request and antiforgery checks, some notification fine and date boundary cases, and the final combined kiosk/API tests still require recorded outcomes. A clean VM installation has also not yet passed. These checks remain open in the assessment checklist.

Email and SMS in this application are simulated records. The notification screens show their recipient and message content; they do not establish delivery through a real messaging provider.

## Evidence locations

Hold workflow test record  |  Circulation integration test record

Patron hold test record  |  Notification test record

## 3 Personal reflection

## Strengths in practical testing

One strength I showed was continuing to investigate when a result did not make sense. A membership number appeared in capital letters on the form but was rejected because I had entered lowercase letters. I reported the difference between what I typed and what the screen displayed, then applied the validation change. This was a useful reminder that a form can look correct while still creating a problem for the person using it.

The repair and transfer journey helped me understand why testing one screen is not enough. Asha's toy was damaged at Darwin and her chosen pickup branch was Palmerston. Repair allowed allocation, but the pickup message had to wait until the destination received the copy. I followed that journey through collection, the next patron's turn and another return requiring repair. Checking those changes together made the purpose of each status clearer to me.

## Working through unfamiliar integration

My main weakness was limited confidence with Git and Entity Framework integration. I needed guidance to understand automatically appearing merge changes, resolve shared files and decide what to commit. I also found it difficult to keep the documentation current while testing. Asking for help allowed me to continue, but I would reduce this dependence next time by learning the branch and migration workflow before the team begins changing shared models.

## Lessons from testing and coordination

I sometimes found similar looking test steps confusing. One example was a notification screen showing six rows that included both loan confirmations and pickup messages. I had to apply the pickup filter before comparing the count. I learned to state the item, message type and expected state before testing, then report the exact result. A short completion reply alone does not explain every part of a test.

I would agree the shared model and service boundaries earlier, bring the branches together in smaller batches and record each result while the relevant screen is open. I would also keep a dated time log from the beginning. Coordinating four modules made me more aware that having code on separate branches and having one tested release are different stages of the work.

## 4 Value added to the application

The main value of my contribution is continuity of a patron's request. A damaged item can be repaired and moved to the chosen branch while the waiting list remains meaningful. The patron should be told to collect only when that copy is actually ready at the destination. The repair, allocation and pickup evidence demonstrates selected stages of this behaviour.

The authenticated hold form also lets an active linked patron place a request through their own account and see its progress. The queue screenshots show how the private request joins the same staff managed waiting list. Simulated notification history gives staff a way to check what was recorded for that event and investigate an unexpected count.

The value demonstrated here is the connected workflow and its recorded outcomes. Staff time saved and comparative novelty have not been measured.

## 5 Partial retrospective time record

The available activity checkpoints give approximately 19 hours 45 minutes through 15 October 2026 at 09:47 Darwin. I selected a rule that retains gaps of two hours or less and excludes longer gaps. This is a partial estimate of elapsed work sessions, including some waiting and short breaks. It is not a complete active or billable timesheet.

Clock windows are approximate and durations are rounded. The reconstruction uses recoverable message timestamps and dated project checkpoints. Missing sessions, offline work and work after the stated cutoff are outside this subtotal. Each session covers several activities and is counted once. Separate task durations were not recorded.

Time reconstruction and calculation basis

## 6 Contribution status and project records

The hold, notification and patron work has source and selected local evidence on the lead branch. On 15 October I pushed the consolidated documentation update as 9b28459, including attributed member handoffs. The final release still needs the newer member fixes, combined acceptance checks and a successful VM installation. Meeting minutes will record the actual discussions supplied by the participants.

The following records connect this report to the shared project. They also identify the remaining work and preserve the ownership of each member's contribution.

Group work and individual contribution record

Assessment checklist

Remaining work and owners

Screenshot index

## Release integration supplement

On 16 October the combined source added shared-rule manager reports, quantity/year import validation, fine/inventory audit exports, notification-filter retention and production-code regression tests. Automated Linux results are recorded separately from the Windows and VM checks. No additional hours are inferred from these code changes. See [release checks](../RELEASE_CHECKS_16OCT.md) and [partial time reconstruction](../KAMALPREET_TIME_RECONSTRUCTION.md).
