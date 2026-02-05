using SolarCharge.API.Application.Features.Vehicles.Models;
using SolarCharge.API.Infrastructure.Tesla.Responses;

namespace SolarCharge.API.Infrastructure.Tesla.Extensions;

public static class TeslaVehicleExtensions
{
    public static VehicleDto ToDto(this TeslaVehicle teslaVehicle)
    {
        return new VehicleDto(
            teslaVehicle.Id,
            teslaVehicle.DisplayName,
            teslaVehicle.State.ToDto());
    }

    public static VehicleDto ToDto(this TeslaProduct teslaProduct)
    {
        return new VehicleDto(
            teslaProduct.Id,
            teslaProduct.DisplayName,
            VehicleStateDto.Unknown);
    }
    
    private static VehicleStateDto ToDto(this TeslaVehicleState state)
    {
        return state switch
        {
            TeslaVehicleState.Unknown => VehicleStateDto.Unknown,
            TeslaVehicleState.Offline => VehicleStateDto.Offline,
            TeslaVehicleState.Asleep => VehicleStateDto.Asleep,
            TeslaVehicleState.Online => VehicleStateDto.Online,
            _ => VehicleStateDto.Unknown
        };
    }
}