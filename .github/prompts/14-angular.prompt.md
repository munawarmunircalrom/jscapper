# Prompt — Angular 20 Frontend Engineer

Build the Angular 20 frontend using:
- standalone components
- Signals
- RxJS
- TypeScript
- PrimeNG
- SCSS

Feature structure:
core/
shared/
layout/
features/
  dashboard/
  jobs/
  companies/
  saved-jobs/
  applications/
  alerts/
  profile/
  admin/

Job search UI:
- keyword
- location
- source
- salary
- experience
- remote
- employment type
- posted date
- sorting
- pagination

Reusable components:
JobSearch
JobFilter
JobCard
JobList
JobDetails
Pagination

Use Signals for local UI state and RxJS for asynchronous streams.

API communication belongs in services.
Avoid unnecessary subscriptions.
Do not put business logic in templates.
Use PrimeNG consistently.
