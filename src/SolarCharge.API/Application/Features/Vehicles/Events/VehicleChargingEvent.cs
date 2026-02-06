using SolarCharge.API.Application.Shared;

namespace SolarCharge.API.Application.Features.Vehicles.Events;

public sealed record VehicleChargingEvent(string DisplayName) : IDomainEvent;
