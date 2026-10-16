# My Windows release test result

Tester: Kamalpreet, S389008. I ran Test-Release.ps1 -IncludeSqlServer after importing b79faf3469aa09702b547b5d2a7c9318b89ba2ba.

| Check | Actual result |
| --- | --- |
| Application build | Succeeded, 47.5 seconds; no warnings shown |
| Windows test adapter | .NET 10.0.12 |
| Total / succeeded / failed / skipped | 33 / 33 / 0 / 0 |
| Test duration | 47.6 seconds |
| Full invocation including restore/build | 105.8 seconds |

All three real SQL cases passed: rightful Ready owner with a waiting follower, simultaneous checkout producing one active loan, and late return recording an unpaid fine/promoting the next request. Each applies fresh canonical migrations in its own generated test database and cleans up only that database.

The result is supported by my supplied terminal transcript. The local TopEndLibraryHub.Tests/TestResults/group8-windows.trx file was not attached or independently opened. The Linux run separately passed 30 and skipped these three SQL tests. This is not a VM installation pass.
