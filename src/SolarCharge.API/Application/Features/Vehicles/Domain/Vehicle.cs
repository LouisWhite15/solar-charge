using SolarCharge.API.Application.Shared;

namespace SolarCharge.API.Application.Features.Vehicles.Domain;

public sealed record Vehicle : Entity
{
    public long Id { get; init; }
    public string DisplayName { get; init; }
    public VehicleState State { get; set; }
    
    public Vehicle(
        long id,
        string displayName,
        VehicleState state = VehicleState.Unknown)
    {
        Id = id;
        DisplayName = displayName;
        State = state;
    }

    public void SetState(VehicleState vehicleState)
    {
        // Do not trigger state update if the state isn't updating
        if (State == vehicleState)
        {
            return;
        }

        // Do not update the vehicle state if we knew what it was and now we don't
        // i.e., store the last known value
        if (State is not VehicleState.Unknown &&
            vehicleState is VehicleState.Unknown)
        {
            return;
        }
        
        State = vehicleState;
    }
}
