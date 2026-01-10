using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using SolarCharge.API.Infrastructure.InfluxDB;
using SolarCharge.API.Infrastructure.Inverter;
using SolarCharge.API.Infrastructure.Inverter.Fronius;

namespace SolarCharge.API.Application.Features.Inverter;

public static class InverterModule
{
    public static IServiceCollection AddInverter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InverterOptions>(
            configuration.GetSection(InverterOptions.Inverter));
        
        services.Configure<InfluxDbOptions>(
            configuration.GetSection(InfluxDbOptions.InfluxDb));
        
        services.AddTransient<IInverterClient, InverterClient>();
        
        services.AddKeyedTransient<IInverterClient, NoOpInverterClientService>(InverterType.Unknown);
        services.AddKeyedTransient<IInverterClient, FroniusInverterClientService>(InverterType.Fronius);
        
        var featureOptions = configuration.GetSection(FeatureOptions.Features).Get<FeatureOptions>();
        if (featureOptions?.IsInfluxDbEnabled is true)
        {
            services.AddSingleton<IInverterTelemetryRepository, InfluxDbInverterTelemetryRepository>();
        }
        else
        {
            services.AddSingleton<IInverterTelemetryRepository, NoOpInverterTelemetryRepository>();
        }

        return services;
    }
}