using Coravel;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Invocables;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Queries;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Application.Services.Vehicles;
using SolarCharge.API.Application.Services.Vehicles.ChargingStrategies;
using SolarCharge.API.Domain.ValueObjects;

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

        // Mediator
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        
        // Queries
        services.AddScoped<IVehicleQueries, VehicleQueries>();
        
        // Services
        services.AddKeyedTransient<IChargingStrategy, UnknownChargeStateStrategy>(ChargeStateDto.Unknown);
        services.AddKeyedTransient<IChargingStrategy, VehicleChargingStrategy>(ChargeStateDto.Charging);
        services.AddKeyedTransient<IChargingStrategy, VehicleNotChargingStrategy>(ChargeStateDto.Stopped);

        services.AddTransient<IDateTimeOffsetService, DateTimeOffsetService>();

        // Coravel scheduling
        services.AddTransient<WriteInverterStatusInvocable>();
        services.AddTransient<ExecuteChargingStrategyInvocable>();
        services.AddTransient<RefreshTeslaAccessTokenInvocable>();
        services.AddScheduler();
        
        return services;
    }
}