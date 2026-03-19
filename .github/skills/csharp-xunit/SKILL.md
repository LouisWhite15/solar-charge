---
name: csharp-xunit
description: 'Get best practices for XUnit unit testing, including data-driven tests'
---

# XUnit Best Practices

Help write effective unit tests with xUnit, covering both standard and data-driven testing approaches.

## Project Setup
- Use a separate test project with naming convention `[ProjectName].Tests`.
- Reference `Microsoft.NET.Test.Sdk`, `xunit`, and `xunit.runner.visualstudio` packages.
- Create test classes that match the classes being tested.
- Use `dotnet test` for running tests.

## Test Structure
- Use `[Fact]` for simple tests.
- Follow the Arrange-Act-Assert pattern.
- Name tests using the pattern `MethodName_Scenario_ExpectedBehavior`.
- Use constructor setup and `IDisposable.Dispose()` teardown when needed.
- Use `IClassFixture<T>` or `ICollectionFixture<T>` for shared context when helpful.

## Standard Tests
- Keep tests focused on a single behavior.
- Avoid testing multiple behaviors in one test method.
- Use clear assertions that express intent.
- Make tests independent and idempotent.

## Data-Driven Tests
- Use `[Theory]` with `[InlineData]`, `[MemberData]`, or `[ClassData]` as appropriate.
- Use meaningful parameter names in data-driven tests.

## Assertions
- Use the assertion that best communicates intent.
- Use `Assert.Throws` or `Assert.ThrowsAsync` for exception verification.
- Consider fluent assertions if the repository adopts them.

## Mocking and Isolation
- Consider Moq or NSubstitute when mocking dependencies is necessary.
- Use interfaces to facilitate mocking.

## Test Organization
- Group tests by feature or component.
- Use `[Trait]` when categorization adds value.
- Use `ITestOutputHelper` for diagnostics when helpful.
