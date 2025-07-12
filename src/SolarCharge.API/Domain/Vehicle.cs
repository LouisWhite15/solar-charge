using SolarCharge.API.Domain.DomainEvents;
using SolarCharge.API.Domain.SeedWork;

namespace SolarCharge.API.Domain;

public class Vehicle : Entity
{
    public ChargeState ChargeState { get; private set; }
    
    public void StartCharging()
    {
        if (ChargeState is ChargeState.Charging or ChargeState.Charged)
            return;
        
        ChargeState = ChargeState.Charging;
        
        AddDomainEvent(new VehicleStartedChargingEvent(Id));
    }

    public void StopCharging()
    {
        if (ChargeState is ChargeState.Stopped)
            return;

        ChargeState = ChargeState.Stopped;
        
        AddDomainEvent(new VehicleStoppedChargingEvent(Id));
    }
}