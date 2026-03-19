# AGENTS.md

## Purpose
This repository contains a self-hosted solar-aware EV charging system built from two .NET applications:

- `src/SolarCharge.API`: the main web app and orchestration service for Tesla auth, inverter telemetry, charging strategy, and the UI.
- `src/SolarCharge.ChatBot`: a separate Telegram bot service.

When making changes, prefer preserving the existing feature-oriented / vertical-slice layout over introducing new cross-cutting folders.

## Rough architecture

### Solution layout
- `src/SolarCharge.sln`: solution entry point.
- `src/SolarCharge.API/Application`: application layer, organised by feature slices.
- `src/SolarCharge.API/Infrastructure`: integrations and persistence concerns.
- `src/SolarCharge.API/Web`: Razor component UI.
- `src/SolarCharge.ChatBot`: standalone Telegram bot service.

### Vertical slices in the API
The API already follows a feature-first structure under `src/SolarCharge.API/Application/Features`, for example:
- `ChargingStrategy`
- `ChatBot`
- `Inverter`
- `TeslaAuth`
- `Vehicles`

Each feature may contain folders such as:
- `Commands`
- `Queries`
- `Domain`
- `Events`
- `Services`
- `Infrastructure`
- `Models`
- `Extensions`

Keep new work inside the most relevant existing feature. If a change spans multiple concerns, prefer thin integration points between slices rather than creating a new horizontal "shared logic" dumping ground.

## Working rules

### 1) Preserve vertical-slice boundaries
- Add new behaviour to an existing feature slice first.
- Create a new feature folder only when the behaviour is meaningfully distinct.
- Avoid moving feature-specific code into generic top-level utility folders unless it is truly shared by multiple slices.
- Keep UI concerns in `Web`, integration/persistence concerns in `Infrastructure`, and business behaviour in `Application/Features/...`.

### 2) Prefer small, local changes
- Follow existing naming and folder conventions before inventing new abstractions.
- Reuse the current module/registration pattern (`AddApplication`, feature modules, infrastructure modules) when wiring new services.
- Keep constructors, DI registrations, and options close to the feature or integration they belong to.

### 3) Treat generated files carefully
- EF Core migration designer files and snapshots live under `Infrastructure/Data*` or `Infrastructure/Database/Migrations`.
- Do not hand-edit generated migration designer files unless there is a very strong reason.
- If a schema change is required, update the source model first and regenerate migrations in the normal way.

### 4) Respect service boundaries
- `SolarCharge.API` and `SolarCharge.ChatBot` are separate deployable apps. Do not casually couple them together with shared runtime assumptions.
- If logic is only used by one app, keep it there.

## Skills and repository instructions
- Always look for `AGENTS.md` files before changing files, and obey the most specific one in scope.
- If a task clearly matches an available skill, use that skill instead of recreating the workflow from scratch.
- In this repo, skill usage is especially important for tasks related to creating or installing Codex skills.
- Keep loaded context small: read only the files needed to complete the task.

## Practical guidance for future agents
- Start with `README.md` for product intent and local setup.
- Inspect `Program.cs` and feature/module registration to understand how a change is composed.
- When adding a new capability, first decide which feature slice owns it, then add the minimum supporting infrastructure.
- Prefer updating existing tests/checks or adding targeted coverage near the changed behaviour when the repo already has an established pattern.

## When unsure
If the correct placement of code is ambiguous, choose the option that:
1. keeps behaviour inside the owning feature,
2. minimises new cross-feature coupling, and
3. matches the existing folder and registration patterns already present in the repo.
