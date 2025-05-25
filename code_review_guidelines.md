
# GitHub Copilot Code Review Guidelines

## General C# Guidelines

1. **Explicit Typing Over `var`**
   > Avoid using `var` unless the type is obvious from the right-hand side. Prefer explicit types for readability.

2. **Consistent Naming Conventions**
   > Use PascalCase for class names and method names, camelCase for local variables and parameters, and prefix private fields with an underscore.

3. **Avoid Magic Numbers**
   > Replace magic numbers with named constants or enums to improve code readability and maintainability.

4. **Use Expression-Bodied Members Where Appropriate**
   > Use expression-bodied members for simple properties and methods to reduce boilerplate.

## Testing Guidelines

5. **Test Naming Convention**
   > Name test methods using the format `MethodName_StateUnderTest_ExpectedBehavior` for clarity.

6. **One Assertion Per Test**
   > Each unit test should ideally contain a single assertion to isolate failures and improve test clarity.

7. **Mock External Dependencies**
   > Use mocking frameworks to isolate the unit under test from external dependencies like databases or APIs.

## Security Guidelines

8. **Avoid Hardcoded Secrets**
   > Never hardcode API keys, passwords, or connection strings. Use environment variables or secure vaults.

9. **Validate All User Input**
   > Ensure all user input is validated and sanitized to prevent injection attacks.

## Dependency & Configuration

10. **Use Dependency Injection**
   > Prefer constructor injection for dependencies to improve testability and reduce coupling.

11. **Avoid Direct Instantiation of HttpClient**
   > Use IHttpClientFactory to manage HttpClient instances and avoid socket exhaustion.
