# Harsimran Singh individual contribution

Student ID: S388232. Responsibilities: branches, reception desks, staff working location, interbranch transfers, installation guide and VM installation attempt.

## Delivered work

I implemented the Branch and ReceptionDesk models, unique codes, branch migration and repeat-safe seed data, staff working-location selection, item location assignment and transfer dispatch/receipt. Items remain associated with their origin branch during transit and move to their destination on receipt. Ordinary inventory edits preserve physical location; changes between branches use the transfer workflow.

My report supplied on 16 October contains [seven preserved screenshots](../screenshots/harsimran/README.md) from 13 October: dispatch, wrong-branch rejection, destination receipt, database confirmation of Palmerston location, checkout rejection during transit, repeat-receipt rejection and Manager access denial. These are historical component results. The integrated held-item journey is recorded separately in the circulation evidence.

## Confirmed working hours

My confirmed task total is 31 hours, superseding the earlier planning-estimate label in my component report. I did not supply daily start/end times.

| Task | Confirmed hours |
| --- | ---: |
| Branch and reception-desk models | 2 |
| Database migration and seed data | 3 |
| Working-location selection and display | 3 |
| Transfer dispatch and receipt | 5 |
| Checkout and transfer validation | 3 |
| Book, Music and Toy compatibility changes | 3 |
| Functional testing and evidence | 3 |
| Installation guide and report | 3 |
| VM setup and troubleshooting | 4 |
| Integration review and compile checks | 2 |
| Total | 31 |

I include the VM troubleshooting because I performed it even though setup failed. I count this time once.

## Strengths and weaknesses

The strengths of my contribution include implementing a clear dispatch/receipt workflow and invalid-action checks alongside successful transfers. I also ensured my work was visible through feature-branch commits and screenshots.

A weakness was arranging an appropriate VM late. Installation issues prevented successful verification of the guide. Earlier coordination of database and hold-service changes would have provided more time for integration testing.

## Learning

This task helped me understand how a shared model change affects existing application features. The required branch relationship also needed inventory creation/edit location preservation. I learned to retain existing data during migrations, validate staff locations on the server and maintain transfer history.

Reviewing the integration helped me understand why checkout, reservations and transfers share locks and availability rules. I learned to distinguish implemented behaviour, code review and tests supported by actual evidence.

## Value added

My contribution lets staff track item movement across branches, including origin, destination, dispatch time, receipt time and status. The working-location display identifies the selected branch and reception desk. Validation messages explain wrong-branch receipt and checkout during transit. Inventory compatibility changes assign new items a valid branch and preserve existing locations during ordinary edits.

## My installation result

I did not achieve a successful VM installation. I installed Git and .NET on the Darwin VM, but SQL Server 2022 LocalDB failed when SQL Server VSS Writer could not start. Trying with administrator permissions also failed; I have not established the cause. Another environment restricted installation permissions. The combined Windows/SQL tests passed separately and do not replace the blocked VM check.

My original 12-page report retains seven component captures from 13 October and the VM attempt. The confirmed 31-hour table above is my current record.
