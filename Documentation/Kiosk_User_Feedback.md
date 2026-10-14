> Integration note, 14 October: This is Mehakpreet's historical component report from her source branch. Its reported passes and informal participant feedback have not been independently verified here and do not establish a pass for the combined circulation patch. Final private-account, checkout, two-minute timeout and shared-availability tests remain pending.

> Review required: The source report lists Navdeep in its participant table but Princedeep in its feedback/conclusion. Mehakpreet must confirm the actual participant before this report is used in the portfolio; no replacement identity has been inferred.

# Self-Service Kiosk — User Feedback, Improvements and Evaluation Report

**Project:** TopEndLibraryHub
**Unit:** HIT339 — Group 8
**Developer:** Mehakpreet Kaur
**Component:** Self-Service Kiosk and Public Catalogue API

## 1. Introduction

The TopEndLibraryHub self-service kiosk was developed to provide library patrons with an accessible interface for searching library items, filtering catalogue results and checking item availability.

The evaluation process involved reviewing the kiosk's functionality, testing its API endpoints and inviting three users to provide feedback on the completed interface.

## 2. User Testing Participants

Three participants were invited to evaluate the kiosk:

| Participant | Name       |
| ----------- | ---------- |
| User 1      | Anjana     |
| User 2      | Robinpreet |
| User 3      | Navdeep |

The participants reviewed the working kiosk and did not identify any specific issues requiring further changes.

## 3. Improvements Made During Development

### Improvement 1: Catalogue Search and Filtering

**Usability consideration:** Library patrons need a convenient way to locate items without manually browsing the entire catalogue.

**Implemented solution:** The kiosk provides a keyword search field, category filtering, availability filtering and a clear-filters option. These controls allow users to narrow their search and return to the full catalogue when necessary.

**Testing outcome:** Catalogue searching and filtering were tested successfully. The interface displayed matching catalogue entries according to the selected search criteria.

**Benefit:** The implementation makes catalogue browsing more efficient and helps users locate relevant library materials.

### Improvement 2: Handling Invalid Catalogue Searches

**Usability consideration:** When a search does not match any library items, the application needs to handle the result without displaying incorrect catalogue information.

**Implemented solution:** The catalogue API handles searches with no matching results by returning an empty result set. Item lookup also provides an "Item not found" response when an invalid library code is supplied.

**Testing outcome:** A search using `ZZZ999NOTFOUND` returned an empty JSON array. An item lookup using `BK-9999` displayed the "Item not found" message.

**Benefit:** The system handles unsuccessful searches predictably and avoids presenting unrelated items as valid results.

## 4. User Feedback Results

After the kiosk functionality was implemented, three participants were invited to test the interface.

| Tester     | Feedback summary              | Further changes requested |
| ---------- | ----------------------------- | ------------------------- |
| Anjana     | No specific problems reported | None                      |
| Robinpreet | No specific problems reported | None                      |
| Princedeep | No specific problems reported | None                      |

The feedback did not identify additional changes required after the participants' review.

## 5. Functional Testing Summary

| Test case                                | Expected result                    | Observed result   |
| ---------------------------------------- | ---------------------------------- | ----------------- |
| Retrieve catalogue using GET             | Catalogue information returned     | Passed            |
| Search catalogue by keyword              | Matching items returned            | Passed            |
| Search using unmatched keyword           | Empty result set returned          | Passed            |
| Send POST to GET-only catalogue endpoint | Request rejected                   | Passed — HTTP 405 |
| Retrieve item using valid library code   | Matching item information returned | Passed            |
| Retrieve item using invalid library code | Item not found                     | Passed            |

These results provide functional testing evidence for the public catalogue API. A separate review is still required to verify all applicable security and integration requirements.

## 6. Evaluation

The development and testing process helped establish a kiosk interface that supports independent catalogue browsing.

The implemented search and filtering functionality improves catalogue navigation, while predictable handling of unmatched searches provides a more reliable user experience.

The three participants did not request further changes during the informal usability review. However, the absence of reported issues does not guarantee that the application is free from defects.

## 7. Future Recommendations

Future evaluation should include testing the kiosk with a wider range of users, reviewing its mobile responsiveness and verifying its integration with the group's branch, reservation and collection workflows.

## 8. Conclusion

The TopEndLibraryHub self-service kiosk provides catalogue searching, filtering and item availability information through a public interface.

Development improvements and functional API testing have helped strengthen the usability and reliability of the component. Informal feedback from Anjana, Robinpreet and Princedeep did not identify any further modifications required.

The kiosk is ready for final security, integration and submission verification.
