using SolarCharge.API.Application.Services;
using SolarCharge.API.Infrastructure.InfluxDB;

namespace SolarCharge.API.WebApi.Modules;

public static class InfluxDbExtensions
{
    public static IServiceCollection AddInfluxDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InfluxDbOptions>(
            configuration.GetSection(InfluxDbOptions.InfluxDb));
        
        services.AddSingleton<IInfluxDb, InfluxDbService>();
        //services.AddSingleton<IInfluxDb, NoOpInfluxDbService>();
        
        return services;
    }
}