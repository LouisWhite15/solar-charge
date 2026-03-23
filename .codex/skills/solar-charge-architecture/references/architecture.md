# SolarCharge architecture reference

## Solution map
- `src/SolarCharge.sln`: solution entry point.
- `src/SolarCharge.API/Application`: application layer organised by feature slices.
- `src/SolarCharge.API/Infrastructure`: integrations and persistence concerns.
- `src/SolarCharge.API/Web`: Razor component UI.
- `src/SolarCharge.ChatBot`: standalone Telegram bot service.

## Common feature slices
Under `src/SolarCharge.API/Application/Features`, existing slices include:
- `ChargingStrategy`
- `ChatBot`
- `Inverter`
- `TeslaAuth`
- `Vehicles`

A feature may contain folders such as:
- `Commands`
- `Queries`
- `Domain`
- `Events`
- `Services`
- `Infrastructure`
- `Models`
- `Extensions`

## Placement checklist
When a change is ambiguous, prefer the option that:
1. keeps behavior inside the owning feature,
2. minimizes new cross-feature coupling, and
3. matches existing naming, folder, and registration patterns.

## Practical review checklist
- Did the change stay within the owning feature or app?
- Did UI remain in `Web` and persistence/integration remain in `Infrastructure`?
- Did DI/module registration stay close to the feature?
- Did the change avoid new generic utility folders unless logic is truly shared?
- If EF schema changed, was the migration regenerated instead of hand-edited?
