# Room Reservation System

## Overview
This project manages room data (name, capacity, availability, etc.). It records reservations for specific time slots while handling conflicts.

## Architectural Patterns
- **Domain Driven Design (DDD)**: The application is structured around domain concepts, ensuring a clear separation between business logic and infrastructure concerns.
- **CQRS (Command Query Responsibility Segregation)**: Commands and queries are handled separately to improve performance and maintainability.
- **Vertical Slice Architecture**: The application is structured into independent vertical slices, encapsulating features from the UI layer down to the database.
- **Domain Events**: Implements event-driven architecture by raising domain events to notify relevant components of changes in domain state.

## Design Patterns
- **Options Pattern**: Used for managing application configuration settings in a strongly typed manner.
- **Builder Pattern**: Provides a step-by-step way to construct complex objects.
- **Mediator Pattern**: Implements the Mediator pattern using MediatR to decouple handlers from request senders.
- **Repository Pattern**: Encapsulates database access logic and provides an abstraction over data persistence.
- **Unit of Work**: Ensures that multiple repository operations are handled within a single transaction.
- **Disposable Pattern**: Implements IDisposable where necessary to manage unmanaged resources properly.

## Used Frameworks
- **MediatR**: Implements the mediator pattern to handle commands and queries.
- **Entity Framework (Code-First Approach)**: Used for database access with migrations and ORM support.
- **SQLite**: Lightweight database engine used as the data store.
- **FluentValidation**: Provides a fluent API for validating objects and input data.
- **System.CommandLine**: Handles command-line parsing and argument processing.

## Used Package
- **ConsoleTables**: Used for printing responses in a structured table format. More details: [ConsoleTables on NuGet](https://www.nuget.org/packages/ConsoleTables/).

## How to Use?
- Type `-?` in the console to see the available commands, then follow the instructions.
- To exit the application, type `exit`.
- The local database file will be saved next to the compiled files as `rrs_database_lite.db`.

## Requirements
- .NET Version: **8.0**

## Author
Created by: **Dániel Sasvári**

