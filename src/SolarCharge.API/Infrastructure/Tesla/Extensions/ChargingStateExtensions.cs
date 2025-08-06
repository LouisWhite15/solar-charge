using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Infrastructure.Tesla.Extensions;

public static class ChargingStateExtensions
{
    public static ChargeStateDto ToDto(this string chargingState)
    {
        return chargingState switch
        {
            _ => ChargeStateDto.Unknown
        };
    }
}