# Contributing

Thank you for contributing! This document explains how to get started, the preferred workflow, coding standards, and what we expect from pull requests to keep this ASP.NET Core Web API project high quality, secure, and easy to maintain.

If you find a security vulnerability, please follow the instructions in SECURITY.md instead of opening a public issue.

---

## Quick start (local development)

1. Install prerequisites
   - .NET 10 SDK
   - Docker Desktop (or an engine you prefer)
   - Optional: an editor with C# tooling (VS Code, Rider, Visual Studio)

2. Clone the repo and run services
   ```bash
   git clone https://github.com/riteshsingh84/dotnetcore.git
   cd dotnetcore
   docker-compose up -d
   ```

3. Configure local secrets (developer-only)
   ```bash
   # from the Api project folder
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=apidb;Username=apiuser;Password=apipass"
   dotnet user-secrets set "Jwt:Key" "dev-key-of-32+chars-or-better"
   ```

4. Apply EF migrations and seed data
   ```bash
   dotnet ef database update --project src/Infrastructure --context AppDbContext
   ```

5. Run the API
   ```bash
   dotnet run --project src/Api
   ```

6. Run tests
   ```bash
   dotnet test
   ```

---

## Branching & workflow

- Branches:
  - `main` — production-ready releases (protected).
  - `develop` — integration branch (protected).
  - `feature/*` — feature branches for work (based off `develop`).
  - `hotfix/*` — urgent fixes merged to `main` then `develop`.

- Pull Requests:
  - Open PRs against `develop` for regular work or `main` for release/hotfix PRs.
  - Keep PRs focused and small — easier to review.
  - Include a clear description, motivation, and related issue(s).
  - Link to relevant design docs or Copilot Space prompts if used to generate code.

- Merge policy:
  - Require at least one approving review from a maintainer.
  - All CI checks must pass (build, tests, analyzers, security scans).
  - Squash commits when merging unless there is a reason to preserve history.

---

## Commit messages

We recommend using Conventional Commits for clarity:
```
feat(cart): add coupon code support
fix(auth): handle token refresh edge case
chore(deps): update Newtonsoft.Json to 13.0.1
```

---

## Coding standards & expectations

This project follows the conventions and rules in COPILOT_SPACE.md. Quick highlights:

- Clean Architecture: Core, Application, Infrastructure, Api projects.
- Controllers should be thin; business logic belongs in Application services.
- Prefer async/await for I/O-bound operations.
- Nullable reference types enabled.
- Use DTOs for API boundaries (do not return EF entities).
- Use constructor injection for dependencies.
- Prefer C# aliases: `int`, `long`, `string`, `bool`.
- Avoid `new Guid()` — use `Guid.NewGuid()` for new identifiers.
- Use `throw;` to rethrow exceptions to preserve stack traces.
- Use IHttpClientFactory for HTTP clients.

Formatting & static analysis
- The repository config includes Directory.Build.props with analyzers and TreatWarningsAsErrors in CI. Fix warnings and analyzer issues locally before opening a PR.
- Run:
  ```bash
  dotnet format
  dotnet build -p:RunAnalyzers=true
  ```

---

## Tests

- Unit tests: xUnit + Moq + FluentAssertions.
- Integration tests: WebApplicationFactory or containerized DB.
- Test naming: `MethodName_StateUnderTest_ExpectedBehavior`.
- Prefer one assertion per test when practical.
- Ensure tests are deterministic and do not require external services unless explicitly mocked or run via containers.

Before opening a PR, run:
```bash
dotnet test --no-build --verbosity normal
```

---

## Database migrations

- Create migrations in the Infrastructure project:
  ```bash
  dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api
  ```
- Apply migrations in CI or during deploy using `dotnet ef database update` or an automated migration step at startup (if adopted).
- Include SQL/data migration notes in your PR if significant schema changes are introduced.

---

## Secrets & configuration

- Never commit secrets or credentials.
- Use secret stores:
  - Local: `dotnet user-secrets` or environment variables.
  - CI: GitHub Actions Secrets / Azure Pipelines secure variables.
  - Prod: Azure Key Vault / AWS Secrets Manager / HashiCorp Vault.
- Add `.env.example` or instructions in the README describing required configuration keys.

---

## Security & vulnerability reporting

- Do not open public bug reports for confirmed security issues.
- See SECURITY.md for the preferred secure reporting process.
- We use dependency scanning (Dependabot, etc.) and SAST where configured; respond to alerts promptly.

---

## Pull request checklist

Make sure your PR addresses the following (add a checklist to the PR description):

- [ ] The code builds locally and CI is green.
- [ ] New logic has unit and/or integration tests where appropriate.
- [ ] Tests are passing.
- [ ] New public APIs are documented (Swagger XML comments).
- [ ] No secrets or sensitive data in the diff.
- [ ] Static analyzers and formatting applied.
- [ ] Security considerations documented (threats/risk and mitigation).
- [ ] Database migration created (if schema changes).
- [ ] Update docs (.md) if behavior or configuration changed.

Use the Review Rubric in COPILOT_SPACE.md when reviewing or requesting reviews.

---

## Using Copilot Space & prompt pack

This repository includes a Copilot Space with ready-to-use prompts for scaffolding controllers, EF Core, JWT, tests, and more. When you use prompts to generate code:

- Review generated code thoroughly (logic, validation, security).
- Add tests and documentation for generated features.
- Note which prompt was used in the PR description for traceability.

---

## Adding or updating dependencies

- Add new NuGet packages via PR and explain why the dependency is needed.
- Keep the dependency list minimal and prefer well-maintained libraries.
- If the dependency is security-sensitive, include rationale and mitigation (e.g., why it's safe, update policy).

---

## Issues & feature requests

- Search existing issues before opening a new one.
- For bug reports include reproduction steps, logs, and environment (commit/tag).
- For feature requests include a short proposal, motivating use case, and acceptance criteria.

---

## Code of Conduct & security

- All contributors must follow the Contributor Covenant (see CODE_OF_CONDUCT.md).
- For security reporting follow SECURITY.md (do not post sensitive information publicly).

---

## Need help?

- Maintainer: `@riteshsingh84`
- If your PR is blocked for more than 48 hours, ping a maintainer in the PR or add a comment requesting review.

---

Thank you for contributing — your efforts make this project better and more secure. If you’d like, we can add ISSUE_TEMPLATE and PULL_REQUEST_TEMPLATE files to automate the checklist; tell me if you want those created and I’ll generate them.