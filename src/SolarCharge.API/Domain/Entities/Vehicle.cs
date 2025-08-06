using SolarCharge.API.Domain.DomainEvents;
using SolarCharge.API.Domain.SeedWork;
using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Domain.Entities;

public sealed record Vehicle : Entity
{
    public long Id { get; init; }
    public string DisplayName { get; init; }
    public ChargeState ChargeState { get; private set; }

    public Vehicle(
        long id,
        string displayName,
        ChargeState chargeState = ChargeState.Unknown)
    {
        Id = id;
        DisplayName = displayName;
        ChargeState = chargeState;
        
        AddDomainEvent(new VehicleCreatedEvent
        {
            Id = Id,
            DisplayName = DisplayName,
            IsCharging = ChargeState is ChargeState.Charging
        });
    }

    public void SetChargeState(ChargeState chargeState)
    {
        if (ChargeState == chargeState)
        {
            return;
        }
        
        ChargeState = chargeState;
        
        AddDomainEvent(new VehicleChargeStateUpdatedEvent
        {
            Id = Id,
            ChargeState = ChargeState
        });
    }
}
