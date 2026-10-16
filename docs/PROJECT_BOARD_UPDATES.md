# Finish the GitHub project board

This is an action guide for the group leader. It is not included in the portfolio PDF.

Board: https://github.com/users/Kamal03102006/projects/1

The public review on 16 October found exactly four items, no hidden next page, three Todo and one In Progress. All Actual Hours and Target Date fields were empty. The board has read access from this environment, so these online edits must be made in your signed-in GitHub account.

## 1. Correct the four existing items

Open View 1 or View 2 in Table layout. Click each Status cell and select the appropriate option.

| Existing issue | Current status | Update |
| --- | --- | --- |
| #1 Agree team responsibilities and availability | Todo | Done. The group agreement and all four individual handoffs are in the portfolio. |
| #2 Requirements and acceptance for all seven modules | In Progress | Done. Link docs/REQUIREMENTS.md and the portfolio requirements section. |
| #3 Verify every member can clone, build and run | Todo | In Progress until all four confirm this check. The lead's 33-test run alone does not establish every member's environment. |
| #4 Shared database, borrowing, holds and transfers | Todo | Done. Link the seven canonical migrations and docs/REQUIREMENTS.md. |

If a completed issue is closed, also check its board Status is Done. Keep #3 open until its four-member check is confirmed. Include Harsimran (harsimran-s20) in the team-wide task assignments. The other usernames are Kamal03102006, preetmehakkaur9-lab and bhattarai-sandesh.

## 2. Add the work missing from the board

Use Add item to create each missing task, or add the existing pull request when a link is shown. Record the owner, status and evidence below. For a draft task, convert it to a repository issue if needed to assign a member and retain its description. These are closeout entries created now; do not give them invented historical creation dates.

| Task title | Owner | Status | Evidence to link in the item |
| --- | --- | --- | --- |
| Self-service kiosk and private patron account | Mehakpreet | Done | PR #8; Documentation/Kiosk_Integration_Readme.md; PH01-PH03 evidence. |
| Secure read-only catalogue API | Mehakpreet | Done | docs/test-evidence/windows-api-result-16oct.md; ten passing API checks. |
| Simulated loan, reminder and pickup notifications | Kamalpreet | Done | Notification services; C08/C11 captures; event/channel duplicate protection. |
| Branches, reception desks and item transfers | Harsimran | Done | PR #6; docs/screenshots/harsimran/README.md. |
| Holds, FIFO queue and destination pickup | Kamalpreet | Done | H01-H03 and C07-C13 captures; real SQL circulation tests. |
| CSV book importer | Sandesh | Done | Combined release: validation/quantity tests; actual three-copy/repeat/header checks. |
| Manager dashboard and CSV/PDF audits | Sandesh | Done | Combined release: before/after CSV and PDF; AUD 27 unpaid-to-paid reconciliation. |
| Combined Windows, SQL and API acceptance | Kamalpreet with module owners | Done | docs/RELEASE_CHECKS_16OCT.md; 33 Windows tests and ten API checks. |
| Installation and patron guides | Harsimran and Mehakpreet | Done | docs/INSTALLATION_GUIDE.md; Documentation/PatronUserGuide.md. |
| Verify installation on a separate VM | Harsimran | In Progress / Blocked | LocalDB Error 1920 and SQL Server VSS Writer startup failure. Do not mark Done. |
| Individual records and portfolio | All four; Kamalpreet compiles | Done | docs/HIT339_Group8_Portfolio.pdf and docs/individual/. |
| Publish and merge the final integration release | Kamalpreet | In Progress | integration/group8-release-16oct. Mark Done after the final push and reviewed merge. |
| Submit portfolio PDF and repository link | Kamalpreet | Todo | Mark Done only after Blackboard confirms the submission. |

The existing Status options are Todo, In Progress and Done. For the VM, keep In Progress and put Blocked: LocalDB installation failed at the start of the description, or add a Blocked status option. A failed installation attempt is not a completed VM verification.

Sandesh's PR #7 was still open at this review, although its source history is incorporated in the combined release. After the final integration branch is pushed and merged, close or resolve #7 with a link to that integration. Avoid re-merging the older branch over the tested release.

## 3. Hours and dates

Use Estimated hours only for the documented estimates. Put confirmed task hours in Actual Hours where supported by the member's record. Do not enter each member's whole total on multiple tasks: that would multiply it. The detailed hour allocations are already in the individual portfolio sections. Leave task-level values blank where they were not recorded rather than inventing a breakdown. Use 16 October as a closeout target only where appropriate, not as a fabricated completion timestamp.

## 4. Final checks

Switch to Board layout grouped by Status and keep the Table view for owners, dates and hours. Check that completed tasks appear under Done, the VM remains visibly blocked, and the final source publication/submission tasks reflect their actual outcomes. Confirm the project is still public/readable for marking. GitHub normally saves field edits immediately; use Save view if the layout shows unsaved view changes.

The portfolio's task register is a factual snapshot of the 16 October review. It does not claim these online edits have already been made.
