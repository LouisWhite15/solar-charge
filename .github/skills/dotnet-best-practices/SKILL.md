---
name: dotnet-best-practices
description: 'Ensure .NET/C# code meets best practices for the solution/project.'
---

# .NET/C# Best Practices

Ensure .NET/C# code in the current selection meets the best practices specific to this solution/project.

## Documentation and Structure
- Create XML documentation comments for public classes, interfaces, methods, and properties when the project already uses that style or when public API discoverability benefits from it.
- Include parameter descriptions and return value descriptions in XML comments when added.
- Follow the established namespace and folder structure already used by the repository.

## Design Patterns and Architecture
- Prefer the architectural patterns already present in the solution.
- Use interface segregation with clear naming conventions.
- Keep classes focused, cohesive, and aligned with SOLID principles.
- Follow existing command, query, hosted service, and infrastructure patterns where applicable.

## Dependency Injection and Services
- Use constructor dependency injection.
- Register services with appropriate lifetimes.
- Prefer `Microsoft.Extensions.DependencyInjection` patterns already established in the project.
- Implement service interfaces where they improve testability or separation of concerns.

## Async/Await Patterns
- Use async/await for I/O operations and long-running tasks.
- Return `Task` or `Task<T>` from async methods.
- Use `ConfigureAwait(false)` where appropriate for library-style code.
- Handle async exceptions properly.

## Testing Standards
- Follow the repository's testing framework and conventions.
- Use the Arrange/Act/Assert structure without necessarily adding comments.
- Test both success and failure scenarios.
- Include null or invalid-parameter validation tests where relevant.

## Configuration and Settings
- Use strongly typed configuration classes when appropriate.
- Prefer configuration binding patterns already present in the repository.
- Support `appsettings.json`-based configuration.

## Error Handling and Logging
- Use structured logging.
- Include meaningful log context.
- Throw specific exceptions with descriptive messages.
- Use try/catch blocks for expected failure scenarios.

## Performance and Security
- Use modern C# and .NET features appropriate to the project target framework.
- Implement proper input validation and sanitization.
- Prefer parameterized queries and ORM-safe patterns for database operations.
- Follow secure coding practices.

## Code Quality
- Ensure SOLID principles compliance.
- Avoid code duplication through reusable abstractions where warranted.
- Use meaningful names that reflect domain concepts.
- Keep methods focused and cohesive.
- Implement proper disposal patterns for resources.
