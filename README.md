# Task Management System

# How to run the repository:

The repository includes 4 projects: 
Service1, Service2, Service3 - are .Net projects.
Ta9DB - is SQL Server project.
To run the projects rebuild each solution and run the services using HTTP.
To connect to Entity Framework Database you can change the connection string in launchSettings.json
and run Update-Database command in Package Manager Console.

# Link to generated Swagger/OpenAPI documentation for Service 1: http://localhost:5000/swagger

# Overview

This project is a microservices-based Task Management System.
It is split into multiple services, each with a well defined responsibility, following Clean Architecture principles.


# The solution demonstrates:

1. Separation of concerns across services.
2. Use of ASP.NET Core Web API for HTTP endpoints.
3. Use of Entity Framework Core for database access.
4. Communication between services via HTTP APIs and WebSockets.


# Service 1 – Task Manager (API Gateway)

# Language & Framework: C# with ASP.NET Core Web API, .Net 8 framework

# Purpose:

1. Acts as the entry point for clients (frontend or external apps).
2. Provides HTTP endpoints for creating, updating, and deleting tasks.
3. Forwards commands to Service 2 via WebSocket communication.

# Why ASP.NET Core?

1. Strong support for Web API development.
2. Easy integration with Swagger for API documentation.
3. First-class support for WebSocket connections.


# Service 2 – Task Processor

# Language & Framework: C# with ASP.NET Core, .Net 8 framework

# Database: Microsoft SQL Server with Entity Framework Core

# Purpose:

1. Responsible for business logic and persistence.
2. Implements the repository pattern with EF Core to handle database CRUD.
3. Processes commands received from Service 1 via WebSockets.

# Why EF Core + SQL Server?

1. EF Core simplifies ORM mapping and supports LINQ queries.
2. SQL Server is reliable, scalable, and integrates seamlessly with EF Core.


# Service 3 – Task Analyzer

# Language & Framework: C# with ASP.NET Core Web API, .Net 8 framework

# Purpose:

1. Provides analysis endpoints for tasks.
Example: Given a task ID, calculates the maximum tree depth (longest parent-child chain).
Returns results to Service 2 via HTTP calls.

# Why a separate service?

1. Separation of analysis logic from CRUD logic keeps the architecture modular and scalable.
2. Other services can reuse analysis without duplicating code.
3. Service 3 uses the logic and data layers from Service 2 to keep architecture modular and scalable.


# Communication Between Services:

# Service 1 to Service 2 (Processor):

# Communication: WebSockets

# Example flow:

A client sends HTTP request /tasks to Service 1.
Service 1 serializes the command and forwards it to Service 2 via WebSocket.
Service 2 gets the command via WebSocket and executes the command and updates the database.

# Service 2 to Service 3 (Analysis):

# Communication: HTTP (REST API)

# Example flow:

Service 2 needs to calculate the maximum tree level for a task.
Service 2 sends GET /analyze-task/{id}/level to Service 3.
Service 3 queries the database, calculates the tree depth, and returns a formatted message string.