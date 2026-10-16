# Remaining work for the combined release

Updated 16 October. All seven required feature areas have combined implementation source. The code builds and automated fixture/model/export checks run as recorded in [release checks](RELEASE_CHECKS_16OCT.md). This is not a completed VM installation or a final submission declaration.

## Finish order

1. Import the prepared integration branch into the local repository. Stop the running application and build the solution. Keep the existing seven-migration database if its history matches; otherwise select a separate fresh demonstration database as explained in INSTALLATION_GUIDE.md.
2. Run `scripts/Test-Release.ps1 -IncludeSqlServer` on Windows with LocalDB. The three SQL Server checks use newly generated test databases; they cover fresh migration, rightful reserved pickup with a follower, simultaneous checkout and late-return queue/fine handling. Retain the TRX results. A successful run is not inferred from the Linux fixture tests.
3. Run the local application. Verify one import and its row diagnostics; compare Manager dashboard/CSV/PDF figures; run the API script with the actual local secret; finish the targeted role, private-account and notification checks in RELEASE_CHECKS_16OCT.md. Earlier unchanged hold screenshots need not be recreated.
4. Complete the installation instructions on a suitable VM, or obtain the lecturer's agreement to an alternative. The current LocalDB VM attempt failed. A local build and SQL test run do not replace the required VM check.
5. Confirm Mehakpreet's revised time breakdown and Sandesh's final unrecorded duration. Harsimran's 31 hours are confirmed. Clarify the third feedback participant only if that named entry is retained; otherwise retain the consistently identified member-reported reviewers and acknowledge the unresolved entry.
6. Assemble one Group Portfolio PDF from the prepared requirements, agreement, team approach/plan/task records, minutes, four individual accounts, installation/patron guides, diagrams/journeys and selected evidence. Update actual test statuses and final source revision, inspect every exported page, and verify repository/project-board read access.
7. Push the combined integration branch, review/merge it into master after the Windows checks and submit the PDF plus final repository link. The official deadline remains 16 October 23:59 Darwin unless a written extension is received.

## Member actions

| Member | Remaining action |
| --- | --- |
| Kamalpreet | Bring in the integration release, coordinate final checks, update later personal time where recoverable, and assemble/submit the portfolio |
| Harsimran | Help with final Windows transfer/installation checks; supply the blocked VM evidence and finish a successful VM run if a suitable environment becomes available |
| Mehakpreet | Confirm the corrected hours proposal, resolve the participant-name discrepancy if using that entry, and help verify final kiosk/API/private-session behaviour |
| Sandesh | Confirm final time spent on testing/documentation/fixes and help run the corrected importer/report checks; raw representative samples and synthetic export examples are now included |
| Group | Record actual outcomes, complete final PDF and check marker access |

The group has the individual contribution/reflection material. Remaining work is integration acceptance, the VM requirement, final record confirmation and packaging. No exact completion percentage or predicted mark is assigned.
