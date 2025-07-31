using Coravel;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Invokables;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Api.Modules;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<ApplicationOptions>(
            configuration.GetSection(ApplicationOptions.Application));
        
        services.Configure<FeatureOptions>(
            configuration.GetSection(FeatureOptions.Features));
        
        services.AddHttpClient();
        
        // Services
        services.AddKeyedTransient<IChargingStrategy, UnknownChargeStateStrategy>(ChargeState.Unknown);
        services.AddKeyedTransient<IChargingStrategy, VehicleChargingStrategy>(ChargeState.Charging);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(ChargeState.Stopped);

        // Coravel scheduling
        services.AddTransient<WriteInverterStatusInvokable>();
        services.AddTransient<EvaluateSolarGenerationInvokable>();
        services.AddScheduler();
        
        return services;
    }
}