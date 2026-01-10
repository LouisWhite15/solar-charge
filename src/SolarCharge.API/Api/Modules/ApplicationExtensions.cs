using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Application.Services.ChargingStrategies;
using SolarCharge.API.Application.Shared;
using SolarCharge.API.Infrastructure.HostedServices;

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
        
        // Shared
        services.AddTransient<IClock, Clock>();
        
        // Services
        services.AddKeyedTransient<IChargingStrategy, UnknownChargeStateStrategy>(VehicleStateDto.Unknown);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Offline);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Asleep);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Online);
        
        services.AddSingleton<INotificationService, NotificationService>();

        // Hosted Services
        services.AddHostedService<InverterTelemetryHostedService>();
        services.AddHostedService<ExecuteChargingStrategyHostedService>();
        services.AddHostedService<RefreshTeslaAccessTokenHostedService>();
        
        return services;
    }
}