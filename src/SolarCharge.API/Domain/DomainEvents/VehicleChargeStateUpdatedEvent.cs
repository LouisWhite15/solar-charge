using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Domain.DomainEvents;

public class VehicleChargeStateUpdatedEvent : IDomainEvent
{
    public long Id { get; init; }
    public ChargeState ChargeState { get; init; }
}