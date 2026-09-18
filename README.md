# UserDirectoryApi

## Project overview

`UserDirectoryApi` is a small, layered ASP.NET Core Web API that manages a simple user directory. The solution follows a clean, domain-oriented structure with the following projects (logical):

- `UserDirectory.Api` — ASP.NET Core Web API surface, controllers, middleware and static-file hosting for a potential SPA.
- `UserDirectory.Application` — application services, DTOs, AutoMapper profiles, and validation registrations.
- `UserDirectory.Infrastructure` — EF Core-based persistence, repository implementations, and dependency injection for infrastructure concerns.
- `UserDirectory.Domain` — domain entities and value objects.

Primary responsibilities:
- Expose REST endpoints for CRUD operations on `User` entities.
- Persist data using SQLite (file-based by default) via EF Core.
- Provide consistent error handling and logging (Serilog).
- Include unit and integration tests (NUnit) for repository and controller behaviors.

## Quickstart — prerequisites

- .NET 8 SDK (install from https://dotnet.microsoft.com)
- Optional but recommended: `dotnet-ef` tool for migrations

Install `dotnet-ef` (global):


## Configure connection string

By default the application uses a file-based SQLite database at `./data/userdirectory.db`. You can override via `appsettings.json` or environment variable:

- `ConnectionStrings:DefaultConnection` — e.g. `Data Source=./data/userdirectory.db` 

The application will create the `data` folder at startup if it does not exist.

## Applying database migrations

Create and apply migrations locally (example):


If you prefer automatic migration at startup, the project includes a recommended pattern to apply migrations programmatically (calling `db.Database.Migrate()` during startup). Ensure the `Microsoft.EntityFrameworkCore` namespace is imported where this call is used.

## Run the API


By default the API will expose endpoints under `/api/users`. Swagger UI is enabled in Development environment and available at `/swagger`.

## Running tests

Unit and integration tests use NUnit. From the solution root:


Unit tests use EF Core InMemory; integration tests run the API using `WebApplicationFactory<Program>` and an in-memory SQLite connection.

## Technology stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8 with SQLite provider
- AutoMapper
- FluentValidation
- Serilog for structured logging
- Swashbuckle (Swagger) for API documentation
- NUnit for test framework, FluentAssertions for assertions, Moq for mocking

## AI tools utilized during development

- GitHub Copilot — assisted with code generation, refactor suggestions, and test scaffolding.


## Architecture and design decisions

1. Layered/Clean structure
   - The solution separates concerns into `Api`, `Application`, `Infrastructure`, and `Domain` layers. This keeps controllers thin (API concerns), business rules in application layer, persistence in infrastructure, and core models in domain.

2. Dependency injection and composition
   - Each layer exposes an extension method (e.g. `AddInfrastructure`, `AddApplication`) that registers services and keeps `Program.cs` composition clean.

3. EF Core + SQLite
   - SQLite is chosen for simplicity and zero-configuration local development (file-based database). EF Core 8 is used to maintain compatibility with .NET 8.
   - Migrations are supported via `dotnet ef` and programmatic application at startup using `db.Database.Migrate()`.

4. Logging and error handling
   - Serilog is used for structured logging and configured to read from app configuration.
   - A centralized `ExceptionMiddleware` returns `application/problem+json` responses and logs unexpected errors.

5. Validation and mapping
   - `FluentValidation` registers validators from the `Application` assembly.
   - `AutoMapper` profiles centralize DTO-to-entity mappings.

6. Testing strategy
   - Unit tests: Use EF InMemory provider to test repository logic in isolation.
   - Controller unit tests: Mock repository dependencies with `Moq` and verify controller behavior.
   - Integration tests: Exercise the real HTTP pipeline using `WebApplicationFactory<Program>` with an in-memory SQLite connection to validate end-to-end behavior and schema interactions.

## Troubleshooting

- "Unable to retrieve project metadata. Ensure it's an SDK-style project." — Ensure each `.csproj` has a top-level `<Project Sdk=\"Microsoft.NET.Sdk\">` element.
- `Migrate()` not found — Add `using Microsoft.EntityFrameworkCore;` where `db.Database.Migrate()` is called and ensure EF Core packages are referenced.
- "unable to open database file" — Verify connection string path and process permissions for the `data` folder.
- If native SQLite interop errors occur, ensure `Microsoft.Data.Sqlite` and `Microsoft.EntityFrameworkCore.Sqlite` versions match and are compatible with .NET 8.

## Project layout

- `src/UserDirectory.Api` — API project
- `src/UserDirectory.Application` — Application layer
- `src/UserDirectory.Infrastructure` — Persistence and infrastructure
- `src/UserDirectory.Domain` — Domain entities
- `tests` — Unit and integration tests

## Frontend (Web SPA)

Overview

A lightweight React single-page application (SPA) is included in `UserDirectory.Web`. The front end is built with Vite and React and is intended to be a development SPA that proxys API calls to the local API server.

Location

- Frontend source: `UserDirectoryApi/UserDirectory.Web`
- Dev entry: `UserDirectoryApi/UserDirectory.Web/index.html`
- Vite entry: `src/main.jsx`

Technology

- React (v19)
- Vite (development/build tool)
- React Router

Dev server

From the repository root run:


- Vite dev server runs on port `5173` by default.
- The Vite config proxies requests under `/api` to the API at `http://localhost:5290`. Ensure the API is running on that port (or update the proxy and CORS settings accordingly).

Build

To produce a production build:


Proxy and CORS

- The Vite config (`vite.config.js`) contains a proxy entry that forwards `/api` calls to `http://localhost:5290` (the API). Edit `target` if your API runs on a different host/port.
- The API registers a permissive CORS policy named `ReactPolicy` allowing any origin, header and method. For production, narrow this to the SPA origin.


Troubleshooting

- If API calls from the frontend return CORS errors, ensure the API is running and `ReactPolicy` allows the SPA origin or use `AllowAnyOrigin` for local development.
- If the proxy is not forwarding, verify Vite is running on port `5173` and the `target` in `vite.config.js` points to the running API.

Recommended next steps

- Pin the SPA origin in the API CORS policy for production.
- Add authentication handling in the SPA (login, token storage using in-memory or secure storage, token refresh if used).

---


