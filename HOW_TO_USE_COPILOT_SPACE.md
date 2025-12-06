# How to Use This Copilot Space

## Quick Guide
1. **Review Goals & Tech Stack**  
   Understand the objectives and baseline technologies before coding.

2. **Follow Architecture & Security Guidelines**  
   Apply Clean Architecture, keep controllers thin, enforce JWT security.

3. **Use Prompt Pack in Copilot Chat**  
   Copy prompts from the **Prompt Pack** section to quickly scaffold:
   - Solution structure
   - Controllers
   - EF Core setup
   - Authentication
   - Logging & health checks
   - Unit & integration tests

4. **Maintain Structure & Conventions**  
   Organize code as shown in the **Structure** section and follow naming rules.

5. **Collaborate & Review**  
   Share this Space with your team, use the review rubric for PRs, and keep docs updated.

---

## Sample Usage Scenario
**Goal:** Add a new `ProductsController` with CRUD endpoints.

**Steps:**
1. Open Copilot Chat in your IDE.
2. Paste this prompt:
   ```
   Generate ProductsController with CRUD endpoints using DTOs and validators.
   ```
3. Copilot will:
   - Create `ProductsController` in `/src/Api/Controllers`.
   - Implement endpoints: `GET`, `POST`, `PUT`, `DELETE`.
   - Use DTOs and FluentValidation.
4. Review code against **Architecture & Security** guidelines.
5. Add tests using:
   ```
   Create xUnit tests for ProductService and integration tests using WebApplicationFactory.
   ```
6. Commit and push for review using the **Review Rubric**.
