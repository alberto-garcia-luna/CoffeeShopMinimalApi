Summary
-

The goal of this project is to generate a simple example of an API, based on MinimalAPI technology, that retrieves information from a relational database using clean architecture and CQRS (Command and Query Responsibility Segregation) patterns.

Content
-

The solution is made up of four projects, which are described below:
- CoffeeShopMinimalApi

  Main project exposing 2 endpoints with CRUD functions for the Coffee and Ingredients database objects.

  As mentioned before, it is based on OpenAPI technology and exposes Swagger documentation using the Swashbuckle middlaware for Asp .Net
- CoffeeShopMinimalApi.Infrastructure

  Project where the database elements reside; this project is based on a CQRS architecture and uses the Microsoft Entity Framework ORM to generate and update the database in the SQL Server engine
- CoffeeShopMinimalApi.Tests

  Project where we can find the unit and integration tests of the solution. Unit tests take advantage of the Moq testing framework to mock dependency objects with the database, while integration tests use an in-memory database generated on the fly to test the solution code without affecting the SQL Server database.
- CoffeeShopMinimalApi.TestContainers

  Test project that uses the TestContainers testing framework to generate a container on which integration tests are performed in a SQL server engine similar to what we can find in the Azure cloud but running in our local Docker environment
