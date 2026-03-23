---
name: csharp-xunit
description: Use this skill when adding or updating xUnit tests so test naming, structure, fixtures, and data-driven patterns match repository conventions.
---

# xUnit guidance

Use this skill when writing or reviewing tests.

## Test structure
- Prefer one behavior per test.
- Follow Arrange / Act / Assert.
- Name tests like `MethodName_Scenario_ExpectedBehavior`.
- Use `[Fact]` for simple tests and `[Theory]` for data-driven scenarios.

## Test design
- Keep tests independent and idempotent.
- Match test classes to the class or feature under test.
- Use constructor setup, `IDisposable`, `IClassFixture<T>`, or `ICollectionFixture<T>` only when shared context genuinely helps.
- Use `Assert.Throws` or `Assert.ThrowsAsync` for exception assertions.
- Use `ITestOutputHelper` when additional diagnostics are useful.
