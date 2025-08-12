using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Domain.DomainEvents;

public class VehicleStateUpdatedEvent : IDomainEvent
{
    public long Id { get; init; }
    public VehicleState State { get; init; }
}