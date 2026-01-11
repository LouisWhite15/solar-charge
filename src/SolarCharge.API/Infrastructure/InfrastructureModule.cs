using SolarCharge.API.Infrastructure.HostedServices;

namespace SolarCharge.API.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.Configure<InfrastructureOptions>(
            configuration.GetSection(InfrastructureOptions.SectionName));
        
        // Hosted Services
        services.AddHostedService<InverterTelemetryHostedService>();
        services.AddHostedService<ExecuteChargingStrategyHostedService>();
        services.AddHostedService<RefreshTeslaAccessTokenHostedService>();

        return services;
    }
}