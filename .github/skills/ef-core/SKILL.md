---
name: ef-core
description: 'Get best practices for Entity Framework Core'
---

# Entity Framework Core Best Practices

Help follow best practices when working with Entity Framework Core.

## Data Context Design
- Keep `DbContext` classes focused and cohesive.
- Use constructor injection for configuration options.
- Override `OnModelCreating` for fluent API configuration.
- Separate entity configurations using `IEntityTypeConfiguration<T>`.
- Consider `DbContextFactory` patterns where appropriate.

## Entity Design
- Use meaningful primary keys.
- Implement proper relationships.
- Use data annotations or fluent API for constraints and validations as appropriate.
- Implement navigational properties deliberately.

## Performance
- Use `AsNoTracking()` for read-only queries where appropriate.
- Implement pagination for large result sets.
- Use `Include()` deliberately to avoid N+1 query issues.
- Prefer projection when only a subset of fields is required.
- Consider compiled queries for frequently executed paths.

## Migrations
- Create small, focused migrations.
- Name migrations descriptively.
- Verify generated migration SQL before production rollout.
- Add data seeding through migrations when appropriate.

## Querying
- Use `IQueryable` judiciously and understand deferred execution.
- Prefer strongly typed LINQ queries over raw SQL.
- Use appropriate query operators for readability and efficiency.

## Change Tracking and Saving
- Use appropriate change tracking strategies.
- Batch `SaveChanges()` calls intentionally.
- Implement concurrency control for multi-user scenarios when needed.
- Consider transactions for multiple related operations.
- Use appropriate `DbContext` lifetimes.

## Security and Testing
- Avoid SQL injection by using parameterized queries and safe ORM patterns.
- Be careful with raw SQL queries.
- Use provider choices intentionally in tests (in-memory vs. SQLite).
- Test migrations in isolated environments when practical.
