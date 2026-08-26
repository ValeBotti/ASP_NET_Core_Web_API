# ASP.NET-Core-Web-API

[![C# Language](https://img.shields.io/badge/C%23-Language-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/web-api/)

Backend created for the food delivery app.  

**The problem:** My Kotlin app couldn't retrieve data anymore because the web app had been shut down.<br>
That meant I couldn't see my application working properly, and that's why I decided to start this project.

> I've studied and created this project by myself;
> This very basic project is a nice exercise that allows me to move beyond the stage where everything simply works to a project that will leave me with a set of valuable lessons on a complex backend framework.

# REFACTORING:

#### 1. ABSTRACTION BY SPECIFICATION / CONTRACT
When looking at my project, I couldn't be satisfied with my "OrderController" class; I knew that my methods were taking way too many responsibilities, and even I couldn't comprehend it at first glance.<br>
The first thing I thought was that I clearly wasn't using the framework's standard architecture in the best way, so I wanted to add a few comments to help me out in the process of **separation of concerns**.<br>

Right away, I remembered my OOP class in Java and Liskov's book:
**abstraction by specification**.

📚 [Barbara Liskov & John Guttag — *Program Development in Java:
Abstraction, Specification, and Object-Oriented Design*](https://www.oreilly.com/library/view/program-development-in/9780768685299/)

Thanks to ChatGPT, I've learned about the existence of XML Documentation comments, and that's what I've used to add my Liskov-friendly comments.

> My comments specify the effects and modifications of those methods. The method signature makes returns explicit (my signatures needed refactoring too), and for my sanity's sake, I'll assume there are no errors in my methods to specify.


// ## OVERVIEW: Layered Web API Architecture
