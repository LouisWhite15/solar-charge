using SolarCharge.API.Application.Features.Vehicles.Events;
using SolarCharge.API.Application.Shared;

namespace SolarCharge.API.Application.Features.Vehicles.Domain;

public sealed record Vehicle(
    long Id,
    string DisplayName,
    VehicleState State,
    bool IsCharging,
    DateTimeOffset LastUpdated)
    : Entity
{
    public VehicleState State { get; set; } = State;
    public bool IsCharging { get; set; } = IsCharging;
    public DateTimeOffset LastUpdated { get; set; } = LastUpdated;

    public void ApplyTelemetry(VehicleTelemetry telemetry)
    {
        UpdateState(telemetry.State, telemetry.Timestamp);
        UpdateChargingState(telemetry.IsCharging, telemetry.Timestamp);
    }
    
    private void UpdateState(VehicleState updatedVehicleState, DateTimeOffset now)
    {
        if (now < LastUpdated)
        {
            // Ignore out-of-order updates
            return;
        }
        
        if (State is not VehicleState.Unknown &&
            updatedVehicleState is VehicleState.Unknown)
        {
            // Retain existing state if the update is unknown
            return;
        }
        
        if (State == updatedVehicleState)
        {
            // Do not trigger state update if the state isn't updating
            return;
        }
        
        State = updatedVehicleState;
        LastUpdated = now;
    }
    
    private void UpdateChargingState(bool isCharging, DateTimeOffset now)
    {
        if (now < LastUpdated)
        {
            // Ignore out-of-order updates
            return;
        }
        
        if (IsCharging == isCharging)
        {
            // Do not trigger state update if the state isn't updating
            return;
        }
        
        IsCharging = isCharging;
        LastUpdated = now;
        
        if (IsCharging)
        {
            AddDomainEvent(new VehicleChargingEvent(DisplayName));
        }
        else
        {
            AddDomainEvent(new VehicleNotChargingEvent(DisplayName));
        }
    }
}
