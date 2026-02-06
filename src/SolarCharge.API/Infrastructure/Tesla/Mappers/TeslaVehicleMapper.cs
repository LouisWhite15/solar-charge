using SolarCharge.API.Application.Features.Vehicles.Models;
using SolarCharge.API.Infrastructure.Tesla.Responses;

namespace SolarCharge.API.Infrastructure.Tesla.Mappers;

public static class TeslaVehicleMapper
{
    public static VehicleDto ToDto(TeslaProduct teslaProduct)
    {
        return new VehicleDto(
            teslaProduct.Id,
            teslaProduct.DisplayName,
            VehicleStateDto.Unknown,
            false);
    }
    
    public static VehicleDto ToDto(TeslaVehicle teslaVehicle)
    {
        return new VehicleDto(
            teslaVehicle.Id,
            teslaVehicle.DisplayName,
            teslaVehicle.State.ToDto(),
            false);
    }

    public static VehicleDto ToDto(TeslaVehicle teslaVehicle, TeslaVehicleData teslaVehicleData)
    {
        return new VehicleDto(
            teslaVehicle.Id,
            teslaVehicle.DisplayName,
            teslaVehicle.State.ToDto(),
            teslaVehicleData.ChargeState.ChargingState is TeslaChargingState.Charging);
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