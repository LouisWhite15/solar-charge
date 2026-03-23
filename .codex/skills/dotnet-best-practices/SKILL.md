---
name: dotnet-best-practices
description: Use this skill for general .NET and C# implementation work to preserve the repository's structure, dependency injection, configuration, logging, and code quality conventions.
---

# .NET implementation guidance

Use this skill for general .NET/C# changes.

## Architecture and structure
- Follow the repository's existing namespaces, folder structure, and patterns.
- Prefer cohesive classes and feature-aligned abstractions.
- Use constructor injection and register services with appropriate lifetimes.

## Implementation guidance
- Use async/await for I/O and long-running work.
- Prefer strongly typed configuration when configuration objects already exist or improve clarity.
- Use structured logging with meaningful context.
- Throw specific, descriptive exceptions.
- Avoid duplication unless a shared abstraction is clearly warranted.

## Quality bar
- Use clear domain-oriented naming.
- Keep methods focused.
- Apply secure coding and input validation.
- Follow established testing conventions when adding coverage.
