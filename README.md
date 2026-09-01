# ASP.NET-Core-Web-API - DEVELOPMENT JOURNAL

[![C# Language](https://img.shields.io/badge/C%23-Language-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/web-api/)

Backend created for the food delivery app.  

**The problem:** My Kotlin app couldn't retrieve data anymore because the web app had been shut down.<br>
That meant I couldn't see my application working properly, and that's why I decided to start this project.

> I've studied and created this project by myself;
> This very basic project is a nice exercise that allows me to move beyond the stage where everything simply works to a project that will leave me with a set of valuable lessons on a complex backend framework.

# REFACTORING:

## 1. ABSTRACTION BY SPECIFICATION / (DESIGN BY CONTRACT)
When looking at my project, I couldn't be satisfied with my "OrderController" class; I knew that my methods were taking on way too many responsibilities, and even I couldn't comprehend it at first glance.<br>
The first thing I thought was that I clearly wasn't using the framework's standard architecture in the best way, so I wanted to add a few comments to help me out in the process of **separation of concerns**.<br>

Right away, I remembered my OOP class in Java and Liskov's book:
**abstraction by specification**.

📚 [Barbara Liskov & John Guttag — *Program Development in Java:
Abstraction, Specification, and Object-Oriented Design*](https://www.oreilly.com/library/view/program-development-in/9780768685299/)

Thanks to ChatGPT, I've learned about the existence of XML Documentation comments, and that's what I've used to add my Liskov-friendly comments.

> My comments specify the effects and modifications of those methods. The method's signature makes returns explicit (my signatures needed refactoring too), and for my sanity's sake, I'll assume there are no errors in my methods to specify.

**Let's go over what Liskov said in her book about procedural abstractions:** <br>
- **headers / (method signatures):** "gives the name of the procedure, the number, order, and types of its parameters and the type of its results" "it's similar to the "form" of a mathematical function, as in 
f: integer->integer".<br>
- **requires:** "The requires clause states the constraints under which the abstraction is defined. The requires clause is needed if the procedure is partial; if the procedure is total, it can be omitted".<br>
- **modifies:** "The modifies clause lists the names of any inputs that are modified by the procedure. If some inputs are modified, we say the procedure has a side effect. The modifies clause can be omitted when no inputs are modified".<br>
- **effects:** "The effects clause describes the behavior of the procedure for all inputs not ruled out by the requires clause. It must define what outputs are produced and also what modifications are made to the inputs listed in the modifies clause. The effect clause is written under the assumption that the requires clause is satisfied, and it says nothing about the procedure's behavior when the requires clause is not satisfied".<br>

## 2. DEFINING THE DOMAIN AND ENTITIES' ROLES - BACK TO THE E-R SCHEMA
After defining the specifications of the Controller operations, the next step is to refactor the application by introducing a **Service Layer** and a **Repository layer**.
Where should someone start? I believed I needed to rewrite controllers, but the more I looked into it, the further I went from the controller implementation...

> The ASP.NET Core Web API framework is pushing me down to the data layer; probably it's trivial for someone who already knows the framework, but it's not that obvious when you're learning it from scratch.
> So I decided to look into it because it couldn’t be random.
> That’s when I found other interesting concepts: the **dependency rule**, **domain‑driven design**, and **onion architecture**, which ASP.NET Core Web API naturally encourages by design.
> This might be a good time to read Clean Architecture by Robert C. Martin…
> -> Apparently, this framework, like Spring Boot and Laravel, can be described as an **opinionated framework**; that means it follows a set of rules that came from several decades of software engineering research. Interesting, isn't it?

-> Moral of the story: I went back to the **Domain layer**.

Does the domain layer depend on anything? Technically, no. But to build it properly, you must have a clear idea of your data model - the conceptual model; let's get back to it.<br>
Now I desperately need my DB abstraction, and I regret my laziness when I chose not to draw it!
I chose to prioritize seeing my Kotlin app working, and saw the flip side of the coin of the "outcome-oriented" approach, neglecting abstraction.<br>
Should I regret it? I think, in this case, the goal was simply to move forward. Would I do it again? Yes.

### Descriptive documentation

**Entities:**
- **UidSid —** the session identity used by the client.
- **User -** the person using the app.
- **Menu -** the plate available for purchase.
- **Order -** the purchase made by a user.

**Relationships:**
- **UidSid - identifies -> User -** a UidSid identifies the session of a specific User (1, 1).
- **User - is identified by -> UidSid -** a User is identified by at least one UidSid (1, N).
- **User - places -> Order -** a User places zero or more Orders (0, N).
- **Order - is placed by -> User -** one Order is placed by one User (1, 1).
- **Order - refers to -> Menu -** each Order refers to one Menu (1, 1).
- **Menu - is referenced by -> Order -** each Menu can be referred to by zero or more Orders (0, N).

### TEENY-TINY E-R SCHEMA
![E-R SCHEMA](img/schema.jpg)

### TEENY-TINY RELATIONAL SCHEMA
```HTML
- uid_sid(🔑 id, 🔗 user_id)

- user(
    🔑 id,
    first_name,
    last_name,
    card_full_name,
    card_number,
    card_expire_month,
    card_expire_year,
    card_cvv
)

- menu(
    🔑 id,
    name,
    price,
    location_lat,
    location_lng,
    image_version,
    image,
    short_description,
    long_description,
    delivery_time
)

- order(
    🔑 id,
    🔗 user_id,
    🔗 menu_id,
    creation_timestamp,
    status,
    delivery_timestamp,
    current_position_lat,
    current_position_lng
)
```
**Domain constraints (with types):**

**uid_sid**
- **sid_uid.id:** string, not null.
- **sid_uid.user_id:** int, not null, user_id > 0.

**user**
- **user.id:** int, not null, id > 0.
- **user.first_name:** string, nullable.
- **user.last_name:** string, nullable.
- **user.card_full_name:** string, nullable.
- **user.card_number:** string, nullable, len = 16.
- **user.card_expire_month:** int, nullable, between 1 and 12.
- **user.card_expire_year:** int, nullable, between the current year and the current year + 5.
- **user.card_cvv:** string, nullable, len = 3.

-> cross-field constraint: (card_expire_year * 12 + card_expire_month) >= (current year * 12 + current month)

**menu**
- **menu.id:** int, not null, id > 0.
- **menu.name:** string, not null.
- **menu.price:** float, not null, price >= 0.
- **menu.location_lat:** float, not null, -90 <= lat <= 90.
- **menu.location_lng:** float, not null, -180 <= lng <= 180.
- **menu.image_version:** int, not null, image_version >= 0.
- **menu.image:** string, not null.
- **menu.short_description:** string, not null.
- **menu.long_description:** string, not null.
- **menu.delivery_time:** int, not null, delivery_time >= 0.

**order**
- **order.id:** int, not null, id > 0.
- **order.user_id:** int, not null, user_id > 0.
- **order.menu_id:** int, not null, menu_id > 0.
- **order.creation_timestamp:** datetime, not null. (assigned by the DB, not the application).
- **order.status:** enum, not null, {ON_DELIVERY, COMPLETED}.
- **order.delivery_timestamp:** datetime, nullable.
- **order.current_position_lat:** float, not null, -90 <= lat <= 90.
- **order.current_position_lng:** float, not null, -180 <= lng <= 180.

-> cross-field constraint: <br>
status = ON_DELIVERY ⟺ delivery_timestamp IS NULL <br>
status = COMPLETED ⟺ delivery_timestamp IS NOT NULL

delivery_timestamp >= creation_timestamp

**Like a function, a DB model is described also by its data's domain; a DB model without its constraints is not wrong; it's a different model.**

## 3. IMPLEMENTATION
### Domain Layer
When I first implemented the domain model, I tried to reconstruct it based on the APIs I remembered using in the frontend. I made another mistake: I modeled the backend starting from the presentation layer, not the domain. The domain model must always be the primary focus and the foundation of the entire architecture.<br>

Here I am, understanding the reason why we have "Models" and "DTOs"; now it makes sense.<br>

-> With the domain model guiding my decisions, something clicked; I’m starting to think like a backend developer, and the UX - which I've always been very fond of - matters less and less. The focus is the data and its meaning, and the final purpose becomes secondary.<br>

- Now I need constraints; I started to think about what could end up in my database from the API. I needed to block any strange input, but it doesn't make any sense to think that way.<br>
Preventing mistakes or malicious inputs: you could think about it all day and  still end up not covering anything.<br>
-> **The point isn't to foresee every possible input; it is to PROTECT the data's domain.** <br>

### Repository Layer <br>
-> Seeder and DB access. <br>

The domain layer is tied to the database, especially in my application where I used the code-first method.
Nothing else should be there. So, where should my seeder class go? The answer is: the repository layer, which is dedicated to "using" (and, in my case, populating) the DB. <br>
Besides the seeder, every data-access operation must be here. It's like creating functions that encapsulate an SQL query. Inputs are query params, and the output is whatever the SQL query returns. <br>
Plus, it's nice to have modularity and replicability within them. <br>

We are in the implementation phase, and Liskov comes in handy again: the importance of a method's signature. <br>
-> The signature says almost everything you need to know about the persistence operation: the needed parameters and the outcome. <br>

Now let's read my controllers' XML documentation; there I'll find what data my API needs to be retrieved. <br>

-> I've decided to remove Liskov's comments from the controller in favor of the repository methods where actions take place.

# OVERVIEW: Layered Web API Architecture - description
#### 1. Domain / Data Layer - Models + DbContext
```
ASP.NET_Core_Web_API
                    └── Data/
                            └── AppDbContext.cs -> map POCOs to  SQL Server naming conventions + constraints + owned types + lazy loading
                    └── Migrations/ -> DB edit history (I dropped the whole previous DB)
                    └── Models/ -> POCOs
                              └── Location.cs
                              └── Menu.cs
                              └── Order.cs
                              └── UidSid.cs
                              └── User.cs
                    └── Program.cs -> DB connection
```
#### 2. Repository Layer - DB access
```
ASP.NET_Core_Web_API
    └── Repository/
            └── Interfaces/
                    └── IMenuRepository.cs
                    └── IOrderRepository.cs
                    └── IUserRepository.cs
                    └── IUidSidRepository.cs
            └── Implementations/
                    └── MenuRepository.cs
                    └── OrderRepository.cs
                    └── UserRepository.cs
                    └── UidSidRepository.cs
            └── Seed/
                    └── MenuSeeder.cs
                    └── menu.json
            └── Images/
                    └── avocado_toast.jpg
                    └── ...
                    .
                    .
                    .
```
#### 3. Application / Service Layer - Service
#### 4. Presentation Layer - Controller
