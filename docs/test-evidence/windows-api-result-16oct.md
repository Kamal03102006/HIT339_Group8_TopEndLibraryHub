# My Windows HTTPS API result

Tester: Kamalpreet, S389008. I used a fresh private local key and ran Test-CatalogueApi.ps1 against https://localhost:7197. All ten checks passed; the script kept the key private.

| Case | HTTP result |
| --- | ---: |
| Missing key / wrong key | 401 / 401 |
| Catalogue / available / categories / status | 200 / 200 / 200 / 200 |
| Invalid limit / invalid category | 400 / 400 |
| Missing item | 404 |
| Read-only POST | 405 |

The script also checks selected private-field names in successful responses. This is regression coverage, not a full penetration test. Status means health/configured branch activity, not live opening hours.
