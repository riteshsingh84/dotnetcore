
# Copilot Space — ASP.NET Core Web API

> Purpose-built workspace to guide Copilot (and teammates) in generating, discussing, and improving an ASP.NET Core API using best practices, clean architecture, and production-ready conventions.

---

## 1) Goals

- Build a secure, testable, maintainable **ASP.NET Core Web API** (target: .NET 10).
- Enforce **Clean Architecture** and **RESTful** conventions.
- Use **EF Core** for data access, **JWT** for authentication, **Swagger** for API docs.
- Ship with CI/CD, health checks, logging, validation, and versioning.
- Provide reusable **prompt packs** for common tasks (controllers, services, auth, DB, tests).

**Non-Goals**
- Front-end UI.
- Legacy .NET (Framework) support.
- Non-HTTP protocols.

---

## 2) Project Context

- **Project name**: `{{ProjectName}}`
- **Business domain**: `{{Domain}}`
- **Repository**: `{{Repo URL}}`
- **Branch strategy**: `main` (release), `develop` (integration), feature branches `feature/*`
- **Issue tracking**: `{{Azure DevOps|GitHub Issues|Jira}}`
- **Environment targets**: `Local`, `Dev`, `Staging`, `Prod`
- **Cloud/Infra**: `{{Azure|AWS|On-prem}}`
- **Database**: `{{SQL Server|PostgreSQL}}`
- **Secrets management**: `{{Azure Key Vault|GitHub Actions Secrets}}`

---

## 3) Tech Stack & Baseline

- **Runtime**: .NET 10 (ASP.NET Core Web API)
- **Data**: Entity Framework Core (Code First, Migrations)
- **Auth**: JWT Bearer, role-based authorization
- **Docs**: Swagger/OpenAPI (Swashbuckle), XML comments
- **Validation**: FluentValidation or DataAnnotations
- **Logging**: ILogger + Serilog (structured logs)
- **Mapping**: AutoMapper
- **Testing**: xUnit + FluentAssertions + Moq
- **Packaging**: Docker (multistage) + Docker Compose
- **Observability**: Health Checks, request logging, correlation IDs

---

## 4) Architecture & Conventions

**Clean Architecture layers**
- `Core` (Domain entities, interfaces, value objects)
- `Application` (Use cases, DTOs, validators, services)
- `Infrastructure` (EF Core, repositories, external services)
- `API` (Controllers, filters, DI wiring, middleware)

**General rules**
- Prefer **async/await** for I/O.
- Nullability enabled (`<Nullable>enable</Nullable>`).
- SOLID, DRY, SRP. No business logic in controllers.
- Use **DTOs** for API boundaries (no direct entity exposure).
- Return standard HTTP codes; use problem details for errors.
- Version APIs (`/api/v1/...`); don’t break contracts.
- Add Proper Comments in code which describe the business logic.
- Declare all member variables at the top of a class.
- Avoid writing very long methods. A method should typically have 1~25 lines of code. If a method has more than 25 lines of code, you must consider re factoring into separate methods.
- Avoid passing too many parameters to a method. If you have more than 4~5 parameters, it is a good candidate to define a class or structure.
- Use short Don’t Use System.Int16.
- Use int Don’t Use System.Int32.
- Use long Don’t Use System.Int64.
- Use string Don’t Use System. String.
- DO use throw to rethrow an exception; rather than throw *exception object* inside a catch block.
- Avoid direct casts. Instead, use the “as” operator and check for null. 
- "new Guid()" should not be used
- Do not make the member variables public or protected. Keep them private and expose public/protected Properties.
Avoid using `var` unless the type is obvious from the right-hand side. Prefer explicit types for readability.

**Use Dependency Injection**
- Prefer constructor injection for dependencies to improve testability and reduce coupling.
- Use IHttpClientFactory to manage HttpClient instances and avoid socket exhaustion.

**Naming**
- Controllers: `*Controller`
- Services: `*Service`
- Repos: `*Repository`
- DTOs: `*Dto`
- Tests: mirror namespaces with `*.Tests`

**Consistent Naming Conventions**
- Use PascalCase for class names and method names, camelCase for local variables and parameters, and prefix private fields with an underscore.
-  Do not use Hungarian notation to name variables. 
-  Use Meaningful, descriptive words to name variables.
- Do not use single character variable names like i, n, s etc. Use names like index, temp.
-  Do not use underscores (_) for local variable names. 
-  Do not use variable names that resemble keywords.
-  Namespace names should follow the standard pattern like: <company name>.<product name>.<top level module>.<bottom level module>
-  File name should match with class name.
-  Use the prefix "I" for interfaces.
---

## 5) Security & Compliance

- JWT authentication; short-lived access tokens, optional refresh tokens.
- Authorization via policies/roles; deny by default.
- Validate inputs (FluentValidation); sanitize outputs.
- No secrets in source; use environment variables or secret stores.
- HTTPS only; HSTS in production.
- Rate limiting/throttling for sensitive endpoints.
- Log **without** PII; use correlation IDs for traceability.
- Never hardcode API keys, passwords, or connection strings. Use environment variables or secure vaults.

---

## 6) Project Structure (recommended)

```
/src
  /Api
    /Controllers
    /Filters
    /Middleware
    /Models
    /Configuration
  /Application
    /Interfaces
    /Services
    /DTOs
    /Validators
  /Core
    /Entities
    /ValueObjects
    /Events
  /Infrastructure
    /Persistence
    /Repositories
    /Migrations

/tests
  /UnitTests
  /IntegrationTests

/docs
  architecture.md
  api-contracts.md

/.github/workflows
  ci.yml
  cd.yml

Dockerfile
docker-compose.yml
global.json
Directory.Build.props
```

---

## 7) Environment & Configuration

**Minimum local setup**
- .NET SDK: 10.x
- Docker Desktop (or engine)
- SQL container via Compose (or local DB)
- User secrets for dev (`dotnet user-secrets`)

**Config keys (examples)**
- `ConnectionStrings__Default`
- `Jwt__Issuer`, `Jwt__Audience`, `Jwt__Key`
- `Serilog__MinimumLevel`, `Serilog__WriteTo`

---

## 8) CI/CD (example)

- **CI**: build → test → lint → security scan → publish artifacts
- **CD**: containerize → push image → deploy to `{{Azure Web App|AKS}}`

**Quality gates**
- All tests pass; mutation score (if used) above threshold.
- Code coverage ≥ {{threshold}}%.
- Static analysis clean (Roslyn analyzers, SonarQube optional).

---

## 9) Definition of Done

- Endpoint documented (Swagger + XML comments).
- Validated inputs; well-defined responses.
- Unit + integration tests added and passing.
- Logs, metrics, and health checks in place.
- Backwards-compatible API changes unless versioned.
- Security reviewed (authZ, secrets, error messages).

---

## 10) Knowledge Links (insert your org refs)

- ASP.NET Core docs: https://learn.microsoft.com/aspnet/core/
- EF Core docs: https://learn.microsoft.com/ef/core/
- Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
- Serilog: https://serilog.net/
- FluentValidation: https://docs.fluentvalidation.net/

---

## 11) Prompt Pack (ready to paste in Copilot Chat)

### A) Bootstrap API (minimal or MVC)

**Generate solution skeleton**
```
Create a .NET 10 Clean Architecture Web API solution with projects: Core, Application, Infrastructure, Api. 
Wire DI in Api for Application and Infrastructure. Enable nullable reference types. Add Directory.Build.props to enforce analyzers and C# language version.
```

**Enable Swagger + XML comments**
```
Add Swagger/OpenAPI with XML comments, `IncludeXmlComments`, and describe authentication with JWT bearer in the OpenAPI doc. 
Add `app.UseSwagger()` and `app.UseSwaggerUI()` gated by environment when needed.
```

**Global exception handling**
```
Add a global exception-handling middleware returning RFC 7807 ProblemDetails. 
Log exceptions with Serilog, include correlation ID. 
Map common exceptions (ValidationException => 400, UnauthorizedAccessException => 401, NotFoundException => 404).
```

### B) Data & EF Core

**EF Core setup**
```
Add DbContext `AppDbContext` with entities Product(Id, Name, Price, Stock) and Category(Id, Name).
Configure relationships and constraints. 
Add repositories via interfaces in Application, concrete implementations in Infrastructure. 
Create initial migration and update database.
```

**Seed data**
```
Add `DataSeeder` that seeds 5 categories and 20 products if database is empty. 
Run on app startup in `Program.cs` with a scoped service.
```

### C) Authentication & Authorization (JWT)

**JWT wiring**
```
Configure JWT bearer authentication with issuer, audience, and symmetric key from configuration. 
Add a `POST /api/v1/auth/login` issuing JWT with claims (sub, name, role). 
Create authorization policies: `CanViewProducts`, `CanManageProducts`. 
Protect `ProductsController` with roles and policies.
```

### D) Controllers & Endpoints

**CRUD controller**
```
Generate `ProductsController` (v1) with CRUD endpoints:
GET /api/v1/products (paged, filter by category, name, price range)
GET /api/v1/products/{id}
POST /api/v1/products
PUT /api/v1/products/{id}
DELETE /api/v1/products/{id}
Use DTOs and validators; return ProblemDetails on errors; include pagination metadata in headers.
```

**Versioning**
```
Add API versioning (v1, v2) with `AspNetCore.Mvc.Versioning`. 
Annotate controllers and expose version in Swagger docs.
```

### E) Validation & Mapping

**FluentValidation + AutoMapper**
```
Add FluentValidation validators for ProductCreateDto, ProductUpdateDto (Name required, Price > 0, Stock >= 0). 
Register validators and AutoMapper profiles. 
Ensure validation errors return 400 with a consistent schema.
Ensure all user input is validated and sanitized to prevent injection attacks.
```

### F) Logging & Observability

**Serilog**
```
Add Serilog with console sink and request logging. 
Enrich logs with correlation ID, user, and request path. 
Set minimum level `Information` and override `Microsoft` to `Warning`.
```

**Health checks**
```
Add `/health` with DB check and a liveness probe. 
Expose `/ready` for readiness, including EF Core database connectivity.
```

### G) Testing

**Testing Guidelines**
- Name test methods using the format `MethodName_StateUnderTest_ExpectedBehavior` for clarity.
- Each unit test should ideally contain a single assertion to isolate failures and improve test clarity.
- Use mocking frameworks to isolate the unit under test from external dependencies like databases or APIs.

**Unit tests**
```
Create xUnit tests for ProductService: 
- GetById returns product
- Create validates and persists
Use Moq and FluentAssertions.
```

**Integration tests**
```
Create WebApplicationFactory-based tests for ProductsController endpoints. 
Use a test database (SQLite in-memory or containerized Postgres) and EF Core migrations.
```

---

## 12) Review Rubric (use in PRs or Copilot Chat)

- **Architecture**: Business logic in Application layer; controllers thin; DI used correctly.
- **Security**: AuthN/AuthZ enforced; no secret leaks; safe error messages.
- **Reliability**: Validation, exception handling, idempotency as needed.
- **Quality**: Tests present and meaningful; coverage acceptable; code analyzers clean.
- **Performance**: Async I/O; efficient queries; pagination for lists.
- **Docs**: Swagger complete; XML comments on public endpoints; README updated.

---

## 13) Onboarding Script (paste into Copilot)

```
I’m setting up the API locally. 
Generate steps to:
1) Install .NET 10 SDK
2) Run `docker-compose up -d` to start the DB
3) Apply EF migrations
4) Configure user-secrets for JWT and connection string
5) Launch the API with Swagger enabled
6) Run unit and integration tests
Return commands and expected outputs.
```

---

## 14) Optional: Starter Configuration Snippets

**Directory.Build.props**
```xml
<Project>
  <PropertyGroup>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest</AnalysisLevel>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
</Project>
```

**docker-compose.yml (example)**
```yaml
version: "3.9"
services:
  db:
    image: postgres:16
    environment:
      POSTGRES_USER: apiuser
      POSTGRES_PASSWORD: apipass
      POSTGRES_DB: apidb
    ports:
      - "5432:5432"
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U apiuser -d apidb"]
      interval: 10s
      timeout: 5s
      retries: 5
```

---

## 15) Space Constraints for Copilot

- **Always** follow Clean Architecture and RESTful conventions.
- **Avoid** obsolete APIs; prefer minimal, modern ASP.NET Core patterns.
- **Explain** changes and provide links to relevant docs when generating code.
- **Include** tests and docs with any new feature prompts.

---

## 16) Maintainers

- **Product Owner**: `Ritesh Singh`
- **Tech Lead**: `Ritesh Singh`
- **Maintainers**: `Ritesh Singh`
- **Contact/Channel**: `{{Teams}}`

---
