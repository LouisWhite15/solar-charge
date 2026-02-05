namespace SolarCharge.API.Application.Features.Vehicles.Domain;

public sealed record VehicleTelemetry(
    VehicleState State,
    bool IsCharging,
    DateTimeOffset Timestamp);
