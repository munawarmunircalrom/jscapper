# Prompt — Job Alerts and Notifications Engineer

Implement user job alerts.

Alert criteria:
- keywords
- location
- skills
- salary
- experience
- employment type
- remote
- selected sources

Matching pipeline:
Canonical Job
-> Alert Matcher
-> Matching User Alerts
-> Notification

Support:
- in-app notifications
- email notifications
- future push notification abstraction

Prevent duplicate notifications.

Track:
NotificationId
UserId
JobId
AlertId
SentAt
Status

Allow users to:
- create
- edit
- pause
- resume
- delete alerts

Add tests for matching, non-matching and duplicate notification scenarios.
