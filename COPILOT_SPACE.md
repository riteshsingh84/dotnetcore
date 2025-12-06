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
15. Observability & runtime hygiene
16. Operational runbooks & runbook snippets
17. API lifecycle & governance
18. Testing policy & targets
19. Prompt Space constraints for Copilot
20. Maintainers

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
- Observability: Health checks, request logging, correlation IDs, OpenTelemetry, metrics

---

## 4) Architecture & conventions (clarified)

**Clean Architecture layers**
- Core (Domain entities, value objects, domain exceptions, interfaces)
- Application (Use cases, DTOs, validators, application services)
- Infrastructure (EF Core, repositories, external service clients)
- Api (Controllers, filters, DI wiring, middleware)

**General rules (corrected & actionable)**
- Prefer async/await for I/O-bound work.
- Enable nullable reference types (`<Nullable>enable</Nullable>`).
- Apply SOLID, DRY, SRP. No business logic in controllers—controllers orchestrate.
- Use DTOs for API boundaries; do not return EF entities directly.
- Return standard HTTP codes; use RFC 7807 ProblemDetails for errors.
- Version APIs (e.g., `/api/v1/...`) and avoid breaking changes; use versioning strategy.
- Keep methods short and focused. If a method exceeds ~25 lines, consider refactoring.
- If a method has more than ~4–5 parameters, consider extracting a parameter object.
- Prefer explicit typing for readability, but allow `var` when the right-hand type is obvious.
- Prefer C# built-in aliases (int, long, string, bool) over System.* types for readability.
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

**Dependency injection**
- Use constructor injection.
- Use IHttpClientFactory for HttpClient instances to avoid socket exhaustion.

**Quick coding clarifications (minor but helpful)**
- Nullable guidance: annotate DTOs and domain types explicitly, prefer non-nullable unless field is optional. Use [Required] on DTOs where appropriate and validate server-side.
- Cancellation: all public async APIs should accept CancellationToken and propagate it.
- var: use when the type is obvious (e.g. var customer = new CustomerDto(...)).

---

## 5) Security & compliance (practical)

**Authentication & tokens**
- Use JWT access tokens with short lifetimes (e.g., 5–15 minutes) and optional refresh tokens.
- Keep symmetric keys at least 32 bytes. Store keys in Key Vault or other secret store.
- Include `kid` in tokens when rotating keys. Support multiple active keys for smooth rotation.
- Refresh token guidance (concrete):
  - Issue refresh tokens with long TTL (e.g., 7–30 days) but store them server-side (database) or as rotating opaque tokens.
  - Use refresh-token rotation: when a refresh token is used, invalidate it and issue a new refresh token. Track chain and revoke if reuse is detected.
  - Revoke refresh tokens on password change, logout, or suspected compromise.

**Key rotation & incident readiness (expanded)**
- Use key identifiers (kid) in JWT headers and support multiple keys so new tokens can be signed with a rotated key and old tokens validated with previous keys during overlap.
- Rotation schedule: define a cadence (e.g., 90 days) and a process:
  1. Create new key and add to secret store.
  2. Add new key to list of valid keys (accept and prefer new kid).
  3. Start issuing tokens with new kid.
  4. After overlap period (e.g., token TTL + few minutes), retire old key.
- Document rollback steps to revoke refresh tokens and rotate secrets quickly if compromise is suspected.
- Include an emergency "revoke-all" capability to invalidate all refresh tokens (e.g., bump a server-side token version or global revocation timestamp).

**Secrets & local dev**
- Never commit secrets. Use:
  - Production: Azure Key Vault / HashiCorp Vault / AWS Secrets Manager.
  - CI: GitHub Actions Secrets / Azure DevOps secure variables.
  - Local: dotnet user-secrets or environment variables for developers.
- Provide a short developer-only README explaining how to set user-secrets to run locally.

**Authorization**
- Deny-by-default. Explicitly allow via roles or policies.
- Prefer fine-grained policies (policy names, claim-based checks) over large role buckets.

**Transport & headers**
- HTTPS only in production; enable HSTS.
- Add security headers (CSP, X-Content-Type-Options, X-Frame-Options) via middleware or reverse proxy.

**Rate limiting & abuse protection**
- Add rate-limiting middleware (ASP.NET Core built-in or reverse-proxy rules) for sensitive endpoints.
- Consider IP and user-level throttles and burst limits.

**Logging & PII**
- Do not log PII. Mask or redact values before logging.
- Use correlation IDs to trace requests across services. Add them to logs and response headers.

**Dependency & code security**
- Enable dependency scanning (Dependabot, Snyk) and SAST in CI.
- Sanitize all inputs and validate server-side; avoid trusting client-supplied data.

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

**Additional repo files to add (recommended)**
- .github/CODEOWNERS — ensure critical reviewers are always requested for PRs
- .github/dependabot.yml — enable dependency update PRs
- .github/PULL_REQUEST_TEMPLATE.md and ISSUE templates — include DoD checklist (see Contributing)
- SECURITY.md, CONTRIBUTING.md (already present)
- .env.example and Dockerfile sample (see Section 7)

Add CODEOWNERS, CONTRIBUTING.md, and SECURITY.md to standardize contribution and vulnerability handling.

---

## 7) Environment & configuration (examples)

**Minimum local setup**
- .NET SDK 10.x
- Docker Desktop (or engine)
- docker-compose to run DBs for integration tests
- dotnet user-secrets for local secrets

**Config keys (recommended)**
- ConnectionStrings__Default
- Jwt__Issuer
- Jwt__Audience
- Jwt__Key (or better: KeyVaultReference)
- Serilog__MinimumLevel
- Serilog__WriteTo
- Feature flags and environment indicators

**Example local user-secrets commands**
```
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=apidb;Username=apiuser;Password=apipass"
dotnet user-secrets set "Jwt:Key" "some-dev-only-key-of-32chars+"
```

**.env.example (add to repo)**
```env
# Example environment variables for local development
# Do NOT commit secrets to the repo; use dotnet user-secrets for dev values

ConnectionStrings__Default=Host=localhost;Port=5432;Database=apidb;Username=apiuser;Password=apipass
Jwt__Issuer=local-api
Jwt__Audience=local-api
Jwt__Key=dev-key-of-32-characters-or-more-please-change
Serilog__MinimumLevel=Information
ASPNETCORE_ENVIRONMENT=Development
```

**Sample Dockerfile recommendation (multistage)**
- Keep an example Dockerfile in repo root, use a slim runtime image, and keep secrets out of the image.

---

## 8) CI/CD (sample & guidance)

**CI pipeline goals**
- build → restore → test → static analysis → security scan → publish artifact

**Sample GitHub Actions CI (expanded & security-hardening)**
- Add CodeQL SAST analysis, Dependabot, secret-scanning, dotnet format, analyzers, and container vulnerability scan (Trivy).
- Example jobs to include:
  - dotnet/format (fail if formatting issues)
  - dotnet build -p:RunAnalyzers=true (run Roslyn analyzers)
  - dotnet test with coverage (upload coverage)
  - CodeQL analysis (SAST)
  - Trivy or GitHub container scanning job for built container images
  - Secret scanning & dependency updates (Dependabot configured separately)

Minimal expanded workflow snippet (conceptual)
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

      - name: Restore
        run: dotnet restore

      - name: Format check
        run: dotnet format --verify-no-changes

      - name: Build & run analyzers
        run: dotnet build --configuration Release -p:RunAnalyzers=true

      - name: Test
        run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

      - name: Upload coverage
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage.cobertura.xml

  codeql:
    uses: github/codeql-action/init@v2
    with:
      languages: csharp
```

**CI matrix examples**
- If your app supports multiple DB providers or target frameworks, add a job matrix:
  - db: [postgres, sqlserver]
  - tfm: [net10]
- Ensure integration tests run in containers matching the matrix entries.

**Branch protection & CD**
- Require passing CI, required approvers, and security checks before merge to protected branches (`main`, `develop`).
- CD: build images in pipeline, scan them, and push to registry only after gating checks pass.

**Quality gates**
- All tests must pass.
- Enforce analyzers; fix high-severity findings before merge.
- Optional: enforce minimum code coverage threshold (example: 70% for unit tests, tuned per project).

**Dependabot configuration (recommendation)**
- Use `.github/dependabot.yml` to keep dependencies up-to-date and to raise PRs for important updates automatically.

---

## 9) Definition of Done

- Endpoint documented (Swagger + XML comments).
- Input validation and consistent error responses (ProblemDetails).
- Unit + integration tests added and passing.
- Logs and health checks in place.
- Backwards-compatible changes or properly versioned breaking changes.
- Security review performed (no secrets, safe error messages).
- CI checks for format, analyzers, SAST, dependency scanning passed.
- Coverage thresholds met if applied (see Section 18).

---

## 10) Knowledge links

- ASP.NET Core docs: https://learn.microsoft.com/aspnet/core/
- EF Core docs: https://learn.microsoft.com/ef/core/
- Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
- Serilog: https://serilog.net/
- FluentValidation: https://docs.fluentvalidation.net/
- OpenTelemetry .NET: https://opentelemetry.io/docs/instrumentation/net/
- GitHub CodeQL: https://securitylab.github.com/tools/codeql/

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

G) CI scaffolding (new)
```
Generate GitHub Actions workflows that include: dotnet format check, build with analyzers, tests with coverage upload, CodeQL analysis, and a container scan job (Trivy). Add a matrix example for DB providers.
```

H) Observability scaffolding (new)
```
Add OpenTelemetry tracing and metrics: configure OTLP exporter, enable ASP.NET Core instrumentation, add correlation id middleware, and expose /metrics for Prometheus. Add recommended Serilog sinks (Console, File, Seq).
```

I) Key rotation & refresh tokens (new)
```
Add a short doc and code scaffolding for refresh token rotation: refresh token entity, refresh endpoint, rotation logic (invalidate on use), and admin revoke endpoint. Include JWT kid handling and support for validating multiple signing keys.
```

---

## 12) Review Rubric

- Architecture: business logic in Application layer; controllers thin; DI correct.
- Security: AuthN/AuthZ enforced; no secret leaks; safe error messages. CI SAST and dependency scans pass.
- Reliability: validation, exception handling, idempotency when required.
- Quality: meaningful tests; analyzers clean; docs updated.
- Performance: async I/O; efficient queries; pagination for lists.
- Observability: traces, metrics, structured logs, correlation id present.
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

C**I snippets (short)**
- Example: add `dotnet format --verify-no-changes` and CodeQL and Trivy jobs as shown in CI guidance.

---

## 15) Observability & runtime hygiene

What to collect
- Traces (OpenTelemetry), Metrics (Prometheus), Logs (Serilog structured logs), Health checks, and Events/Auditing where required.

Concrete recommendations
- OpenTelemetry:
  - Add AspNetCore and HttpClient instrumentation.
  - Export to OTLP (collector) and/or Jaeger/Zipkin in non-prod for tracing.
- Metrics:
  - Expose Prometheus-compatible /metrics endpoint via prometheus-net or OpenTelemetry metrics.
  - Record request durations, error counts, and DB query timings.
- Logs:
  - Use Serilog with structured properties: CorrelationId, RequestId, UserId (when available, not PII).
  - Recommended sinks: Console (development), File (rotating), Seq/Elastic for centralized logging.
  - Configure retention and rotation on storage.
- Correlation IDs:
  - Middleware to generate/propagate Correlation-Id header; include in response headers and logs.
- Example small middleware responsibilities:
  - Ensure correlation ID present (accept incoming or generate).
  - Add to logging context and response header.
- Health checks:
  - Liveness: basic process and env checks.
  - Readiness: DB connectivity, dependent services availability.
  - Probe endpoints: /health/live and /health/ready.

---

## 16) Operational runbooks & runbook snippets

Add short runbooks for common operational tasks and incidents. Keep them in /docs/runbooks.

**1) Database migration (production)**
- Prepare migration in feature branch; create migration scripts and review.
- Apply migration in staging first and validate.
- Use transactional migrations where supported or run in a maintenance window.
- Rollback plan: have the previous backup available; prefer additive migrations. If destructive changes required, run in two-step: deploy code that supports old+new schema, migrate data, then cut over, then remove compatibility code.

**2) Rollback strategy**
- If a release fails:
  - Halt traffic to new release (scale down).
  - Redeploy previous image/tag.
  - Investigate and communicate status.
- Keep short-lived feature toggles to disable problematic features without a full rollback.

**3) Incident response (example)**
- Triage & severity classification.
- Preserve logs & traces (snapshot).
- Rotate affected secrets if suspected compromise.
- If data exposed: follow legal/compliance steps and communication plan (contact PO/Legal).
- Post-incident: run root cause analysis, share summary, and update runbooks.

**4) Key compromise procedure**
- Rotate keys immediately, invalidate refresh tokens, force re-authentication if necessary, publish status and timeline.

Include quick commands and expected outputs in runbooks (e.g., `dotnet ef database update` sample with necessary env var hints).

---

## 17) API lifecycle & governance

**Versioning & deprecation policy**
- Version APIs explicitly (path or header). Example: `/api/v1/...`.
- Deprecation process:
  1. Announce deprecation in API docs and release notes with deprecation date and recommended alternative.
  2. Keep old version available for a defined overlap (e.g., 90 days) unless critical security issues require earlier removal.
  3. Provide automated telemetry to track usage of deprecated endpoints and notify affected consumers if possible.

**Semantic versioning & release notes**
- Adopt SemVer for published artifacts (images, NuGet packages).
- Maintain a changelog (keep a CHANGELOG.md or rely on release notes in GitHub Releases).

**Public API changes & review**
- Require design review for breaking changes: document the change, migration steps, and compatibility tests.

**Governance**
- Decide and document support windows (how long each major API version is supported).
- Require API contract reviews when changing DTOs (breaking vs non-breaking).

---

## 18) Testing policy & targets

**Test types & responsibilities**
- Unit tests: logic in Application layer. Fast and deterministic. Use Moq and FluentAssertions.
- Integration tests: WebApplicationFactory or containerized DB that exercise the full stack. Use known seed data and cleanup.
- End-to-end tests: optional, run in an environment that mirrors production.

**Coverage targets & gates**
- Set a sensible baseline coverage target (example: 70% overall); enforce only if the repository/team agrees. Prefer quality over raw percentage.
- Use coverage to identify risky untested areas and require tests for new logic paths.

**Test matrix & examples**
- Use CI matrix to run integration tests against supported DB providers (e.g., Postgres/SQL Server) to catch provider-specific behavior.
- Example matrix:
  - include OS variants (ubuntu-latest, windows-latest) only if OS-specific behavior expected.
  - include DB provider (postgres, sqlserver).

**Test data & determinism**
- Use factory methods or fixtures for creating test data.
- Prefer in-memory or containerized DBs; ensure deterministic ordering and isolation (database per test class or test run).

**Testing practices**
- Test naming: MethodName_StateUnderTest_ExpectedBehavior.
- Aim for one logical assertion per test when practical; grouping assertions in integration tests is acceptable when it reduces duplication and keeps intent clear.
- Mock external dependencies (HTTP, queues) in unit tests; use real endpoints in integration tests where needed.

---

## 19) Prompt Space constraints for Copilot (reminder)

- Always follow Clean Architecture and RESTful conventions.
- Avoid obsolete APIs; prefer modern minimal APIs and patterns only where appropriate.
- Explain changes and provide links when generating code.
- Include tests and docs with any new feature prompts.
- When generating code that touches security sensitive areas (auth, tokens, secret handling), add an explicit security checklist item to PRs.

---

## 20) Maintainers

- Tech Lead: Ritesh Singh
- Maintainers: @riteshsingh84
- Contact/Channel: @riteshsingh84/lalriteshsingh@gmail.com

