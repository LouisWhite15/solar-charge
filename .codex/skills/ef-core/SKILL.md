---
name: ef-core
description: Use this skill when changing Entity Framework Core models, queries, or migrations so persistence code follows EF Core design and migration best practices.
---

# EF Core guidance

Use this skill for EF Core model, query, and migration work.

## Modeling and context
- Keep `DbContext` responsibilities cohesive.
- Prefer fluent configuration through `IEntityTypeConfiguration<T>` when the project already uses it.
- Model keys, constraints, and relationships deliberately.

## Querying and performance
- Use `AsNoTracking()` for read-only queries when appropriate.
- Prefer projection when only a subset of fields is needed.
- Use `Include(...)` intentionally to avoid N+1 issues.
- Add pagination for large result sets.

## Saving and migrations
- Batch `SaveChanges()` intentionally.
- Consider transactions and concurrency handling when multiple related writes are involved.
- Keep migrations small and descriptive.
- Verify source-model changes first, then regenerate migrations instead of hand-editing generated artifacts.
