---
name: solar-charge-architecture
description: Use this skill when working in the SolarCharge repository to follow the repo's vertical-slice architecture, service boundaries, and implementation conventions.
---

# SolarCharge Repository Guide

Use this skill for any non-trivial change in this repository.

## Quick workflow
1. Read `README.md` for product intent and local setup.
2. Inspect the owning app before editing:
   - `src/SolarCharge.API` for the main web app, orchestration, and UI.
   - `src/SolarCharge.ChatBot` for the Telegram bot service.
3. Place new behavior in the owning vertical slice under `src/SolarCharge.API/Application/Features/...` when the change belongs to the API.
4. Keep infrastructure code in `Infrastructure`, UI code in `Web`, and business behavior in `Application/Features/...`.
5. Prefer a small local change over introducing new shared abstractions.

## Placement rules
- Add behavior to an existing feature slice first.
- Create a new feature only when the behavior is meaningfully distinct.
- Prefer thin integration points between slices instead of shared dumping grounds.
- Keep constructors, DI registrations, options, and module wiring close to the feature or integration that owns them.
- Do not casually couple `SolarCharge.API` and `SolarCharge.ChatBot` together.

## Generated files and migrations
- Treat EF Core migration designer files and model snapshots as generated artifacts.
- If a schema change is needed, update the source model first and regenerate the migration instead of hand-editing generated files.

## Related skills
Load these only when needed:
- `../dotnet-best-practices/SKILL.md` for general .NET implementation guidance.
- `../csharp-async/SKILL.md` for async/await changes.
- `../csharp-xunit/SKILL.md` for tests.
- `../ef-core/SKILL.md` for persistence and migrations.

## Repository reference
For the repo-specific architecture summary and placement checklist, read `references/architecture.md`.
