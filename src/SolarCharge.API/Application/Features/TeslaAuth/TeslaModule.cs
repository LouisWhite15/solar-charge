using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using SolarCharge.API.Application.Features.TeslaAuth.Services;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Repositories;
using SolarCharge.API.Infrastructure.DataAccess.Repositories;
using SolarCharge.API.Infrastructure.Tesla;
using SolarCharge.API.Infrastructure.Tesla.Repositories;

namespace SolarCharge.API.Application.Features.TeslaAuth;

public static class TeslaModule
{
    public static IServiceCollection AddTesla(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TeslaOptions>(
            configuration.GetSection(TeslaOptions.Tesla));
        
        services.AddTransient<ITeslaAuthenticationService, TeslaAuthenticationService>();
        services.AddTransient<ITesla, TeslaService>();
        
        services.AddScoped<ITeslaAuthenticationRepository, TeslaAuthenticationRepository>();
        services.AddTransient<ITeslaAuthenticationClient, TeslaAuthenticationClient>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;
    }
}