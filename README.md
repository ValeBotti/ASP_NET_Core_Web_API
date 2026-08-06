# ASP.NET-Core-Web-API  
Backend created for the food delivery app.  

**The problem:** my Kotlin app couldn't retrieve data anymore because the web app had been shut down.<br>
That meant I couldn't see my application properly working, and that's why I decided to start this project.

- The DB used is SQL Server. To populate the DB, I used EF Core, specifically the "code-first migration" approach, since the entities are very basic; there's no need    to reverse-engineer a hypothetical E-R schema.
  I've decided not to overcomplicate the database to focus on learning ASP.NET Core.

Documentation:
I've decided to dedicate 4 entities for the web app: SidUid, User, Menu, and Order.

**SidUid:** stores the session ID and the User ID. The User ID is the FK that connects the SidUid table with the auto-increment PK we find in the User table.
The session ID identifies the current session the user is using to retrieve their data; the previous web app worked like that: the user entered data every time the session expired.
-> The session ID is created by the web app and stored.
