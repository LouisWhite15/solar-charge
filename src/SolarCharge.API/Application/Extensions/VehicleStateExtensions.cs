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
            VehicleState.Idle => VehicleStateDto.Idle,
            VehicleState.Offline => VehicleStateDto.Offline,
            VehicleState.Online => VehicleStateDto.Online,
            VehicleState.Charging => VehicleStateDto.Charging,
            _ => VehicleStateDto.Unknown
        };
    }
    
    public static VehicleState ToDomain(this VehicleStateDto state)
    {
        return state switch
        {
            VehicleStateDto.Unknown => VehicleState.Unknown,
            VehicleStateDto.Idle => VehicleState.Idle,
            VehicleStateDto.Offline => VehicleState.Offline,
            VehicleStateDto.Online => VehicleState.Online,
            VehicleStateDto.Charging => VehicleState.Charging,
            _ => VehicleState.Unknown
        };
    }
}