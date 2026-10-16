> Integration note, 14 October: This is Mehakpreet's historical component report from her source branch. Its reported passes and informal participant feedback have not been independently verified here and do not establish a pass for the combined circulation patch. Final private-account, checkout, two-minute timeout and shared-availability tests remain pending.

# Self-Service Kiosk and Catalogue API — Testing Report

Project: TopEndLibraryHub
Unit: HIT339 — Group 8
Developer: Mehakpreet Kaur

## 1. Testing Objective

The purpose of testing was to verify that the self-service kiosk
and public catalogue API function correctly, handle invalid inputs
and restrict public access to catalogue information.

## 2. Testing Environment

Application: TopEndLibraryHub
Framework: ASP.NET Core MVC
Language: C#
Database access: Entity Framework Core
Development environment: Visual Studio
Testing tools: Web browser and PowerShell

## 3. Kiosk Functional Testing

| Test | Expected Result | Actual Result | Status |
|---|---|---|---|
| Open kiosk | Kiosk interface loads | Interface loaded | PASS |
| Search catalogue | Matching items displayed | Matching items displayed | PASS |
| Category filter | Results filtered by category | Filter worked | PASS |
| Availability filter | Results filtered by availability | Filter worked | PASS |
| Clear filters | Search filters reset | Filters reset | PASS |

## 4. API Functional Testing

| ID | Test Case | Expected Result | Actual Result | Status |
|---|---|---|---|---|
| API-01 | GET /api/catalogue | Return catalogue records | JSON returned | PASS |
| API-02 | GET /api/catalogue?search=Top | Return matching records | Matching records returned | PASS |
| API-03 | Search for ZZZ999NOTFOUND | Return empty array | Empty array returned | PASS |
| API-04 | POST /api/catalogue | Reject unsupported method | HTTP 405 | PASS |
| API-05 | GET /api/catalogue/BK-0001 | Return matching item | Item returned | PASS |
| API-06 | GET /api/catalogue/BK-9999 | Return item-not-found response | Item not found | PASS |
| API-07 | GET /api/catalogue?limit=101 | Reject invalid limit | Validation error returned | PASS |
| API-08 | Search longer than 100 characters | Reject search | Validation error returned | PASS |
| API-09 | GET /api/catalogue?limit=0 | Reject invalid limit | Validation error returned | PASS |
| API-10 | GET /api/catalogue?limit=2 | Return up to two records | Two records returned | PASS |

## 5. API Security Review

The catalogue controller defines two GET endpoints.

The controller uses explicit response projections to return:
- Library code
- Item name
- Description
- Status
- Availability

Borrower names, contact details and account information are
not included in these projections.

The controller also implements:
- Search length validation
- Result-limit validation
- Invalid library-code handling
- HTTP 404 responses for missing items
- AsNoTracking() database queries
- Cache-Control: no-store response headers

These checks support the read-only design of the public
catalogue API.

The review does not constitute a complete security audit
of the application.

## 6. User Testing

Three participants were invited to review the kiosk:

1. Anjana
2. Robinpreet
3. Navdeep

No specific changes were requested during the informal review.

Further details are recorded in Kiosk_User_Feedback.md.

## 7. Testing Evidence

Supporting evidence includes:
- Kiosk homepage screenshot
- Catalogue search results
- Category and availability filters
- API catalogue JSON response
- API search response
- Empty search response
- HTTP 405 POST rejection
- Valid library-code lookup
- Invalid library-code response
- Input validation responses
- Valid result-limit response

Screenshots should be retained alongside the final
project documentation.

## 8. Limitations

Testing was performed in the local development environment.

The following activities require final verification:
- Integration with other group components
- Deployment configuration
- Complete application security
- Group demonstration workflow

## 9. Conclusion

The self-service kiosk and catalogue API passed the recorded
functional tests.

The API correctly returned catalogue information, supported
searching and item lookup, rejected unsupported POST requests
and handled invalid query parameters.

The component is ready for final integration verification
and group submission preparation.