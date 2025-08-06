using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Domain.Repositories;
using SolarCharge.API.Infrastructure.DataAccess.Repositories;
using SolarCharge.API.Infrastructure.Tesla;
using SolarCharge.API.Infrastructure.Tesla.Repositories;

namespace SolarCharge.API.Api.Modules;

public static class TeslaExtensions
{
    public static IServiceCollection AddTesla(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TeslaOptions>(
            configuration.GetSection(TeslaOptions.Tesla));
        
        services.AddTransient<ITeslaAuthentication, TeslaAuthentication>();
        services.AddTransient<ITesla, TeslaService>();
        
        services.AddScoped<ITeslaAuthenticationRepository, TeslaAuthenticationRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;
    }
}