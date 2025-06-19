using SolarCharge.API.Domain.DomainEvents;
using SolarCharge.API.Domain.SeedWork;

namespace SolarCharge.API.Domain;

public class Vehicle : Entity, IVehicle
{
    public bool IsCharging { get; private set; }
    
    public void StartCharging()
    {
        if (IsCharging)
            return;
        
        IsCharging = true;
        
        AddDomainEvent(new VehicleStartedChargingEvent(Id));
    }

    public void StopCharging()
    {
        if (!IsCharging)
            return;
        
        IsCharging = true;
        
        AddDomainEvent(new VehicleStoppedChargingEvent(Id));
    }
}