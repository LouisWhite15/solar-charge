---
name: csharp-async
description: Use this skill when implementing or reviewing asynchronous C# code so async naming, exception handling, cancellation, and performance patterns stay consistent.
---

# C# async guidance

Use this skill when editing async C# code.

## Core rules
- Suffix async methods with `Async`.
- Return `Task` or `Task<T>`; use `ValueTask` only when there is a clear performance reason.
- Avoid `async void` except for event handlers.
- Do not block on tasks with `.Wait()`, `.Result`, or `.GetAwaiter().GetResult()`.
- Prefer cancellation tokens for long-running or cancelable operations.

## Error handling
- Catch exceptions only when adding meaningful recovery, logging, or context.
- Do not swallow exceptions from awaited operations.
- When manually constructing a failed task, use `Task.FromException(...)`.

## Performance patterns
- Use `Task.WhenAll(...)` for independent concurrent work.
- Use `Task.WhenAny(...)` for timeout or first-completion flows.
- Avoid unnecessary `async` / `await` pass-through wrappers.
- Consider `ConfigureAwait(false)` in library-style code where appropriate.
