using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Infrastructure.Tesla.Extensions;

public static class ChargingStateExtensions
{
    public static ChargeState ToChargeState(this string chargingState)
    {
        return chargingState switch
        {
            _ => ChargeState.Unknown
        };
    }
}