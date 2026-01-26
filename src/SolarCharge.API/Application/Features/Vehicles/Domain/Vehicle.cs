using SolarCharge.API.Application.Features.Vehicles.Events;
using SolarCharge.API.Application.Shared;

namespace SolarCharge.API.Application.Features.Vehicles.Domain;

public sealed record Vehicle(
    long Id,
    string DisplayName,
    VehicleState State,
    DateTimeOffset LastUpdated)
    : Entity
{
    public VehicleState State { get; set; } = State;
    public bool IsCharging { get; set; }
    public DateTimeOffset LastUpdated { get; set; } = LastUpdated;

    public void UpdateState(VehicleState updatedVehicleState, DateTimeOffset now)
    {
        if (now <= LastUpdated)
        {
            // Ignore out-of-order updates
            return;
        }
        
        if (State == updatedVehicleState)
        {
            // Do not trigger state update if the state isn't updating
            return;
        }
        
        if (State is not VehicleState.Unknown &&
            updatedVehicleState is VehicleState.Unknown)
        {
            // Retain existing state if the update is unknown
            return;
        }
        
        InferChargingState(now);
        
        State = updatedVehicleState;
        LastUpdated = now;
    }

    private void InferChargingState(DateTimeOffset now)
    {
        var durationSinceLastUpdate = now - LastUpdated;
        if (State == VehicleState.Online &&
            durationSinceLastUpdate >= TimeSpan.FromMinutes(20))
        {
            // We have to infer at the moment as the API does not provide charging state directly
            // Based on observation, if the vehicle stays online for more than 20 minutes, it is likely charging (either that or being driven)
            IsCharging = true;
            
            AddDomainEvent(new InferredVehicleChargingEvent(DisplayName));
        }
        else
        {
            // Reset charging state if conditions are not met
            IsCharging = false;
            
            AddDomainEvent(new InferredVehicleNotChargingEvent(DisplayName));
        }
    }
}
