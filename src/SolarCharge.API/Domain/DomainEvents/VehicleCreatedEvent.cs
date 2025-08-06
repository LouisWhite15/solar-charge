namespace SolarCharge.API.Domain.DomainEvents;

public class VehicleCreatedEvent : IDomainEvent
{
    public long Id { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public bool IsCharging { get; init; }
}