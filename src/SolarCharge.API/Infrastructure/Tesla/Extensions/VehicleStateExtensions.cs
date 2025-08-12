using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Infrastructure.Tesla.Extensions;

public static class VehicleStateExtensions
{
    public static VehicleStateDto ToDto(this string state)
    {
        return state switch
        {
            _ => VehicleStateDto.Unknown
        };
    }
}