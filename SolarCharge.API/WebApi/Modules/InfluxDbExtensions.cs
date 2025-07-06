using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Interfaces;
using SolarCharge.API.Infrastructure.InfluxDB;

namespace SolarCharge.API.WebApi.Modules;

public static class InfluxDbExtensions
{
    public static IServiceCollection AddInfluxDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var featureOptions = configuration.GetSection(FeatureOptions.Features).Get<FeatureOptions>();
        
        services.Configure<InfluxDbOptions>(
            configuration.GetSection(InfluxDbOptions.InfluxDb));

        if (featureOptions?.IsInfluxDbEnabled is true)
        {
            services.AddSingleton<IInfluxDb, InfluxDbService>();
        }
        else
        {
            services.AddSingleton<IInfluxDb, NoOpInfluxDbService>();
        }
        
        return services;
    }
}