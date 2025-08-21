using SolarCharge.API.Application;
using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.HostedServices;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Queries;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Application.Services.ChargingStrategies;

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
        
        // Queries
        services.AddScoped<IVehicleQueries, VehicleQueries>();
        
        // Services
        services.AddKeyedTransient<IChargingStrategy, UnknownChargeStateStrategy>(VehicleStateDto.Unknown);
        services.AddKeyedTransient<IChargingStrategy, VehicleChargingStrategy>(VehicleStateDto.Charging);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Offline);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Asleep);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Online);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Driving);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(VehicleStateDto.Updating);

        services.AddTransient<IDateTimeOffsetService, DateTimeOffsetService>();
        services.AddSingleton<INotificationService, NotificationService>();

        // Hosted Services
        services.AddHostedService<WriteInverterStatusHostedService>();
        services.AddHostedService<ExecuteChargingStrategyHostedService>();
        services.AddHostedService<RefreshTeslaAccessTokenHostedService>();
        
        return services;
    }
}