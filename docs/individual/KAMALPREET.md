# Kamalpreet - individual contribution and reflection

Student number: S389008 | Role: team lead, holds, simulated notifications and integration

## My contribution

We used my Assignment 2 application as the foundation. I worked on holds and simulated notifications and coordinated shared circulation, branch and patron changes in Visual Studio. My aim was to keep a family's request connected to the condition and physical location of the item, so that repair or transfer would not lose its place or send an early pickup message.

| My allocation | Delivered outcome | Evidence |
| --- | --- | --- |
| Requirements/coordination | Scope, roles, dependencies and shared data decisions. | Requirements, group record and minutes. |
| Holds and queue | One active request per member/item, FIFO, cancellation and one allocation per copy. | H01-H03 and C07-C13. |
| Patron placement | Private Identity-linked request/progress; inactive/unlinked guards. | PH01-PH03 and shared queue captures. |
| Simulated notifications | Loan, reminder, fine and pickup events with duplicate protection. | Notification records and Email/SMS captures. |
| Shared workflow | Repair allocation, destination readiness, protected collection and next-member promotion. | Integrated journey and real SQL tests. |
| Acceptance/evidence | Guided Windows checks, exact outcomes, screenshots and export reconciliation. | 33-test run, ten API checks and actual CSV/PDF files. |
| Interface/portfolio | Green theme, clearer navigation, evidence captions and final documents. | Views/theme source and portfolio. |

My earlier checkpoints include 74668e1, dbbc540, a1e8190, 70db5b7 and 9b28459. On 16 October I imported the combined b79faf3 release. The final documentation and duplicate-export-button cleanup follow that tested calculation/circulation source. Each member's history remains attributed in the repository.

## My testing

I ran the Windows build and Test-Release.ps1 -IncludeSqlServer: 33 passed, zero failed and zero skipped. The three real SQL cases cover rightful pickup collection, simultaneous checkout and late-return/queue handling. All ten HTTPS API checks also passed.

I imported three physical copies from two CSV rows and repeated the file with zero new copies. I checked mixed invalid rows and a missing header and supplied the actual manager CSV/PDF. Both exports agreed on nine items, four generally available and AUD 27 recorded fines. After I recorded the fine as paid, paid became 27 and unpaid zero, while total fines, the separate current estimate and inventory stayed unchanged.

I confirmed signed-out and Patron route restrictions and that a fresh My library account request after Finish asks for login. Earlier inactive/unlinked account captures show the membership guards. I kept captured outcomes separate from my written results and unrun cases.

## My personal reflection


## Strengths in practical testing

One strength I showed was continuing to investigate when a result did not make sense. A membership number appeared in capital letters on the form but was rejected because I had entered lowercase letters. I reported the difference between what I typed and what the screen displayed, then applied the validation change. This was a useful reminder that a form can look correct while still creating a problem for the person using it.

The repair and transfer journey helped me understand why testing one screen is not enough. Asha's toy was damaged at Darwin and her chosen pickup branch was Palmerston. Repair allowed allocation, but the pickup message had to wait until the destination received the copy. I followed that journey through collection, the next patron's turn and another return requiring repair. Checking those changes together made the purpose of each status clearer to me.

## Working through unfamiliar integration

My main weakness was limited confidence with Git and Entity Framework integration. I needed guidance to understand automatically appearing merge changes, resolve shared files and decide what to commit. I also found it difficult to keep the documentation current while testing. Asking for help allowed me to continue, but I would reduce this dependence next time by learning the branch and migration workflow before the team begins changing shared models.

## Lessons from testing and coordination

I sometimes found similar looking test steps confusing. One example was a notification screen showing six rows that included both loan confirmations and pickup messages. I had to apply the pickup filter before comparing the count. I learned to state the item, message type and expected state before testing, then report the exact result. A short completion reply alone does not explain every part of a test.

I would agree the shared model and service boundaries earlier, bring the branches together in smaller batches and record each result while the relevant screen is open. I would also keep a dated time log from the beginning. Coordinating four modules made me more aware that having code on separate branches and having one tested release are different stages of the work.


## My value added

My contribution keeps the pickup request meaningful through repair and physical movement. The patron hears that the copy is ready only when it has arrived at the chosen branch. Asha and Noah's journey made this benefit concrete.

The private hold form lets an active linked member join the same queue that staff manage and follow their own progress. The simulated notification history helps explain an unexpected message count. I also helped bring the separate modules into one release.

## My hours and improvement

My **estimated effort is 30.76 hours** through 16 October 19:29 Darwin. It covers coordination, development, integration, testing, evidence and documentation. My working-session summary follows.

Next time I would agree data contracts earlier, integrate smaller changes, keep a dated task/time log and test the VM guide near the beginning. I would learn the Git/migration workflow before the team changes shared models. The VM installation problem showed me why setup verification needs its own early milestone.
