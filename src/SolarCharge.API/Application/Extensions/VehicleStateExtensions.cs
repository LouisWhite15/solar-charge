using SolarCharge.API.Application.Models;
using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Application.Extensions;

public static class VehicleStateExtensions
{
    public static VehicleStateDto ToDto(this VehicleState state)
    {
        return state switch
        {
            VehicleState.Unknown => VehicleStateDto.Unknown,
            VehicleState.Offline => VehicleStateDto.Offline,
            VehicleState.Asleep => VehicleStateDto.Asleep,
            VehicleState.Online => VehicleStateDto.Online,
            VehicleState.Charging => VehicleStateDto.Charging,
            VehicleState.Driving => VehicleStateDto.Driving,
            VehicleState.Updating => VehicleStateDto.Updating,
            _ => VehicleStateDto.Unknown
        };
    }
    
    public static VehicleState ToDomain(this VehicleStateDto state)
    {
        return state switch
        {
            VehicleStateDto.Unknown => VehicleState.Unknown,
            VehicleStateDto.Offline => VehicleState.Offline,
            VehicleStateDto.Asleep => VehicleState.Asleep,
            VehicleStateDto.Online => VehicleState.Online,
            VehicleStateDto.Charging => VehicleState.Charging,
            VehicleStateDto.Driving => VehicleState.Driving,
            VehicleStateDto.Updating => VehicleState.Updating,
            _ => VehicleState.Unknown
        };
    }
}