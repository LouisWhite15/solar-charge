using SolarCharge.API.Application.Ports;
using SolarCharge.API.Infrastructure.Tesla;
using SolarCharge.API.Infrastructure.Tesla.Repositories;

namespace SolarCharge.API.Api.Modules;

public static class TeslaExtensions
{
    public static IServiceCollection AddTesla(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TeslaOptions>(
            configuration.GetSection(TeslaOptions.Tesla));
        
        services.AddTransient<ITeslaAuthenticationService, TeslaAuthenticationService>();

        services.AddScoped<ITeslaAuthenticationRepository, TeslaAuthenticationRepository>();

        return services;
    }
}