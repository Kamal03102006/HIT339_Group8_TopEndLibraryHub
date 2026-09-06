# Top End Library Hub - Development Log

This log records the planning, development, testing and improvement of the Top End Library Hub application.

| Date | Time Spent | Work Completed | Concepts and Decisions | Testing and Outcome | Next Step |
|---|---|---|---|---|---|
| 4 September 2026 | approximately 45 minutes | Reviewed the assignment brief and marking rubric. Identified the required Admin, Reception, Manager and Public functions. Decided to create a new project instead of modifying the previous MvcMovie application. | Selected ASP.NET Core MVC because it supports object-oriented models, Entity Framework Core, authentication and responsive web interfaces. | Created a requirements checklist covering every section of the brief. | Configure the new project and authentication. |
| 6 September 2026 | approximately 15 minutes so far | Created the TopEndLibraryHub project using ASP.NET Core MVC, .NET 10 LTS and Individual Accounts authentication with HTTPS. | Kept authentication within the application so role-based access can later be implemented for Admin, Reception and Manager accounts. | The untouched application built and opened successfully. The homepage displayed correctly, and Register/Login options were available. | Design the models, relationships and database structure. |