using SolarCharge.API.Application.Models;
using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Application.Extensions;

public static class ChargeStateExtensions
{
    public static ChargeStateDto ToDto(this ChargeState chargeState)
    {
        return chargeState switch
        {
            ChargeState.Unknown => ChargeStateDto.Unknown,
            ChargeState.Charging => ChargeStateDto.Charging,
            ChargeState.Stopped => ChargeStateDto.Stopped,
            ChargeState.Charged => ChargeStateDto.Charged,
            _ => ChargeStateDto.Unknown
        };
    }
    
    public static ChargeState ToDomain(this ChargeStateDto? chargeState)
    {
        if (chargeState is null)
            return ChargeState.Unknown;
        
        return chargeState switch
        {
            ChargeStateDto.Unknown => ChargeState.Unknown,
            ChargeStateDto.Charging => ChargeState.Charging,
            ChargeStateDto.Stopped => ChargeState.Stopped,
            ChargeStateDto.Charged => ChargeState.Charged,
            _ => ChargeState.Unknown
        };
    }
}