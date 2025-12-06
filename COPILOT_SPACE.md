# Copilot Space — ASP.NET Core Web API

> Purpose-built workspace to guide Copilot (and teammates) in generating, discussing, and improving an ASP.NET Core API using best practices, Clean Architecture, and production-ready conventions.

---

Table of Contents
1. Goals
2. Project context & template variables
3. Tech stack & baseline
4. Architecture & conventions (clarified)
5. Security & compliance (practical guidance)
6. Project structure (recommended)
7. Environment & configuration (examples)
8. CI/CD (sample & guidance)
9. Definition of Done
10. Knowledge links
11. Prompt Pack (ready to paste)
12. Review Rubric
13. Onboarding script
14. Starter configuration snippets
15. Space constraints for Copilot
16. Maintainers

---

## 1) Goals

- Build a secure, testable, maintainable **ASP.NET Core Web API** (target: .NET 10).
- Enforce **Clean Architecture** and **RESTful** conventions.
- Use **EF Core** for data access, **JWT** for authentication, **Swagger** for API docs.
- Ship with CI/CD, health checks, structured logging, validation, versioning, and tests.
- Provide reusable **prompt packs** for common tasks (controllers, services, auth, DB, tests).

Non-Goals
- Front-end UI.
- Legacy .NET Framework support.
- Non-HTTP protocols.

---

## 2) Project context & template variables

Use these placeholders when creating a new project. Replace with concrete values early in the project lifecycle.

- ProjectName: e.g., "Acme.Catalog"
- Domain: e.g., "Catalog"
- Repo URL: e.g., "https://github.com/acme/catalog-api"
- Branch strategy: main (release), develop (integration), feature/* (work)
- Issue tracking: Azure DevOps | GitHub Issues | Jira
- Environment targets: Local, Dev, Staging, Prod
- Cloud/Infra: Azure | AWS | On-prem
- Database: SQL Server | PostgreSQL
- Secrets management: Azure Key Vault | HashiCorp Vault | GitHub Actions Secrets

Tip: Add a small README or header at repo creation listing the chosen values.

---

## 3) Tech Stack & Baseline

- Runtime: .NET 10 (ASP.NET Core Web API)
- Data: Entity Framework Core (Code First, Migrations)
- Auth: JWT Bearer, optional refresh tokens, role- and policy-based authorization
- Docs: Swagger/OpenAPI (Swashbuckle), XML comments
- Validation: FluentValidation (preferred) or DataAnnotations
- Logging: ILogger + Serilog (structured logs)
- Mapping: AutoMapper
- Testing: xUnit + FluentAssertions + Moq
- Packaging: Docker (multistage) + docker-compose for local
- Observability: Health checks, request logging, correlation IDs, OpenTelemetry

---

## 4) Architecture & conventions (clarified)

Clean Architecture layers
- Core (Domain entities, value objects, domain exceptions, interfaces)
- Application (Use cases, DTOs, validators, application services)
- Infrastructure (EF Core, repositories, external service clients)
- Api (Controllers, filters, DI wiring, middleware)

General rules (corrected & actionable)
- Prefer async/await for I/O-bound work.
- Enable nullable reference types (`<Nullable>enable</Nullable>`).
- Apply SOLID, DRY, SRP. No business logic in controllers—controllers orchestrate.
- Use DTOs for API boundaries; do not return EF entities directly.
- Return standard HTTP codes; use RFC 7807 ProblemDetails for errors.
- Version APIs (e.g., `/api/v1/...`) and avoid breaking changes; use versioning strategy.
- Keep methods short and focused. If a method exceeds ~25 lines, consider refactoring.
- If a method has more than ~4–5 parameters, consider extracting a parameter object.
- Prefer explicit typing for readability, but allow `var` when the right-hand type is obvious.
- Prefer C# built-in aliases (int, long, string, bool) over System.Int32/System.Int64/System.String for readability. Example: use `int` not `System.Int32`.
- GUIDs:
  - Avoid `new Guid()` — it produces Guid.Empty.
  - Use `Guid.NewGuid()` to create a new random GUID unless you intentionally need Guid.Empty.
- Exception rethrow:
  - Use `throw;` to rethrow preserving the stack trace, not `throw ex;`.
- Avoid direct casts when nullability is uncertain — prefer `as` and null-check or pattern matching.
- Keep fields private and expose properties when needed; prefer readonly where possible.
- Naming:
  - Controllers: `*Controller`
  - Services: `*Service`
  - Repositories: `*Repository`
  - DTOs: `*Dto`
  - Tests: mirror namespaces and append `.Tests`
- Style:
  - PascalCase for types and methods, camelCase for parameters and locals.
  - Prefix private fields with an underscore (`_`).
  - File name should match the primary class name.
  - Use `I` prefix for interfaces (e.g., `IProductRepository`).

Dependency injection
- Use constructor injection.
- Use IHttpClientFactory for HttpClient instances to avoid socket exhaustion.

---

## 5) Security & compliance (practical)

Authentication & tokens
- Use JWT access tokens with short lifetimes (e.g., 5–15 minutes) and optional refresh tokens.
- Keep symmetric keys at least 32 bytes. Store keys in Key Vault or other secret store.
- Include `kid` in tokens when rotating keys. Support multiple active keys for smooth rotation.

Secrets & local dev
- Never commit secrets. Use:
  - Production: Azure Key Vault / HashiCorp Vault / AWS Secrets Manager.
  - CI: GitHub Actions Secrets / Azure DevOps secure variables.
  - Local: dotnet user-secrets or environment variables for developers.
- Provide a short developer-only README explaining how to set user-secrets to run locally.

Authorization
- Deny-by-default. Explicitly allow via roles or policies.
- Prefer fine-grained policies (policy names, claim-based checks) over large role buckets.

Transport & headers
- HTTPS only in production; enable HSTS.
- Add security headers (CSP, X-Content-Type-Options, X-Frame-Options) via middleware or reverse proxy.

Rate limiting & abuse protection
- Add rate-limiting middleware (ASP.NET Core built-in or reverse-proxy rules) for sensitive endpoints.
- Consider IP and user-level throttles and burst limits.

Logging & PII
- Do not log PII. Mask or redact values before logging.
- Use correlation IDs to trace requests across services. Add them to logs and response headers.

Dependency & code security
- Enable dependency scanning (Dependabot, Snyk) and SAST in CI.
- Sanitize all inputs and validate server-side; avoid trusting client-supplied data.

Key rotation & incident readiness
- Document key rotation procedure and how to invalidate refresh tokens in an incident.

---

## 6) Project structure (recommended)

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
    /Exceptions
  /Infrastructure
    /Persistence
    /Repositories
    /ExternalClients
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
Directory.Build.props
```

Add CODEOWNERS, CONTRIBUTING.md, and SECURITY.md to standardize contribution and vulnerability handling.

---

## 7) Environment & configuration (examples)

Minimum local setup
- .NET SDK 10.x
- Docker Desktop (or engine)
- docker-compose to run DBs for integration tests
- dotnet user-secrets for local secrets

Config keys (recommended)
- ConnectionStrings__Default
- Jwt__Issuer
- Jwt__Audience
- Jwt__Key (or better: KeyVaultReference)
- Serilog__MinimumLevel
- Serilog__WriteTo

Example local user-secrets commands
```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=apidb;Username=apiuser;Password=apipass"
dotnet user-secrets set "Jwt:Key" "some-dev-only-key-of-32chars+"
```

---

## 8) CI/CD (sample & guidance)

CI pipeline goals
- build → restore → test → static analysis → security scan → publish artifact

Sample GitHub Actions CI (minimal)
```yaml
name: CI

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.x
      - name: Restore & Build
        run: dotnet build --configuration Release --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
      - name: Run analyzers
        run: dotnet build -p:RunAnalyzers=true
```

CD guidance
- Containerize with Docker multistage builds.
- Push images to a registry (Azure ACR, GitHub Container Registry).
- Use environment-specific deployment pipelines with secrets pulled from vaults.
- Require branch protection for `main` and `develop`: at least one reviewer, successful CI, and passing security checks.

Quality gates
- All tests must pass.
- Enforce analyzers; fix high-severity findings before merge.
- Optional: enforce minimum code coverage threshold if appropriate for the project.

---

## 9) Definition of Done

- Endpoint documented (Swagger + XML comments).
- Input validation and consistent error responses (ProblemDetails).
- Unit + integration tests added and passing.
- Logs and health checks in place.
- Backwards-compatible changes or properly versioned breaking changes.
- Security review performed (no secrets, safe error messages).

---

## 10) Knowledge links

- ASP.NET Core docs: https://learn.microsoft.com/aspnet/core/
- EF Core docs: https://learn.microsoft.com/ef/core/
- Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
- Serilog: https://serilog.net/
- FluentValidation: https://docs.fluentvalidation.net/
- OpenTelemetry .NET: https://opentelemetry.io/docs/instrumentation/net/

---

## 11) Prompt Pack (ready to paste in Copilot Chat)

A) Bootstrap API
```
Create a .NET 10 Clean Architecture Web API with projects: Core, Application, Infrastructure, Api.
Wire DI in Api for Application and Infrastructure, enable nullable reference types, and add Directory.Build.props with analyzers.
```

B) Enable Swagger + XML comments
```
Add Swagger/OpenAPI with XML comments and example JWT security scheme. Add app.UseSwagger and app.UseSwaggerUI gated by environment.
```

C) Global exception handling
```
Add a global exception-handling middleware returning ProblemDetails. Map ValidationException => 400, UnauthorizedAccessException => 401, NotFoundException => 404. Enrich logs with correlation ID.
```

D) EF Core & seeding
```
Add AppDbContext, Product and Category entities, and repositories. Add DataSeeder that seeds data if DB empty and invoke on startup within a scope.
```

E) JWT wiring
```
Configure JWT bearer authentication using configuration values. Add POST /api/v1/auth/login issuing JWT (claims: sub, name, role). Add policies and protect controllers with [Authorize].
```

F) Controllers & tests
```
Generate ProductsController (v1) with CRUD endpoints, DTOs, and FluentValidation. Create xUnit unit tests for services and WebApplicationFactory integration tests for controllers using a test database container.
```

---

## 12) Review Rubric

- Architecture: business logic in Application layer; controllers thin; DI correct.
- Security: AuthN/AuthZ enforced; no secret leaks; safe error messages.
- Reliability: validation, exception handling, idempotency when required.
- Quality: meaningful tests; analyzers clean; docs updated.
- Performance: async I/O; efficient queries; pagination for lists.
- Docs: Swagger complete; XML comments on public endpoints; README updated.

Include a small PR checklist in your PR template referencing this rubric.

---

## 13) Onboarding Script (copiable)

```
1) Install .NET 10 SDK
2) Run docker-compose up -d (starts DB)
3) Configure user-secrets for JWT and connection string (see .env.example)
4) Apply EF migrations: dotnet ef database update --context AppDbContext --project src/Infrastructure
5) Run the API: dotnet run --project src/Api
6) Run tests: dotnet test
```

Provide a `.env.example` and a short "Run locally" README with exact commands for first-time contributors.

---

## 14) Starter configuration snippets

Directory.Build.props
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

docker-compose.yml (Postgres example)
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

Small Program.cs wiring (conceptual)
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuration & secrets
builder.Configuration.AddEnvironmentVariables();

// Serilog, OpenTelemetry, etc. here...

// DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

// Auth & Authorization
builder.Services.AddAuthenticationJwt(builder.Configuration);
builder.Services.AddAuthorization(options => { /* policies */ });

// EF Migrations & seeding run at startup (scoped)
var app = builder.Build();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.Run();
```

---

## 15) Space constraints for Copilot (reminder)

- Always follow Clean Architecture and RESTful conventions.
- Avoid obsolete APIs; prefer modern minimal APIs and patterns only where appropriate.
- Explain changes and provide links when generating code.
- Include tests and docs with any new feature prompts.

---

## 16) Maintainers

- Product Owner: Ritesh Singh
- Tech Lead: Ritesh Singh
- Maintainers: Ritesh Singh
- Contact/Channel: Teams (add channel link)

