using SolarCharge.API.Application.Features.ChargingStrategy.Services;
using SolarCharge.API.Application.Features.Vehicles;

namespace SolarCharge.API.Application.Features.ChargingStrategy;

public static class ChargingStrategyModule
{
    public static IServiceCollection AddChargingStrategy(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ChargingStrategyOptions>(
            configuration.GetSection(ChargingStrategyOptions.SectionName));
        
        services.AddKeyedTransient<IChargingStrategy, UnknownChargeStateStrategy>(VehicleStateDto.Unknown);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Offline);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Asleep);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Online);
        
        return services;
    }
}