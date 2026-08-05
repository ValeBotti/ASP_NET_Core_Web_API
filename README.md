# ASP.NET-Core-Web-API  
Backend created for the food delivery app.  

The problem: my Kotlin app couldn't retrieve data anymore because the server shut down.<br>
That meant I couldn't see my application properly working, and that's why I decided to start this project.

- The DB used is SQL Server. To populate the DB, I used EF Core, specifically the "code-first migration" approach, since the entities are very basic; there's no need    to reverse-engineer a hypothetical E-R schema.
  I've decided not to overcomplicate the database to focus on learning ASP.NET Core.
