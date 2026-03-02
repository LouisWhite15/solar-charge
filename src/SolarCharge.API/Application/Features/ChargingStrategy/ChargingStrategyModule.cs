using SolarCharge.API.Application.Features.ChargingStrategy.Services;

namespace SolarCharge.API.Application.Features.ChargingStrategy;

public static class ChargingStrategyModule
{
    public static IServiceCollection AddChargingStrategy(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ChargingStrategyOptions>(
            configuration.GetSection(ChargingStrategyOptions.SectionName));
        
        services.AddTransient<IChargingStrategy, VehicleNotChargingStrategy>();
        services.AddTransient<IChargingStrategy, VehicleChargingStrategy>();
        
        return services;
    }
}