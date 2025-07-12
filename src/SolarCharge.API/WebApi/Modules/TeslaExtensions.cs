using SolarCharge.API.Application.Interfaces;
using SolarCharge.API.Infrastructure.Tesla;

namespace SolarCharge.API.WebApi.Modules;

public static class TeslaExtensions
{
    public static IServiceCollection AddTesla(this IServiceCollection services)
    {
        services.AddTransient<ITeslaAuthService, TeslaAuthService>();

        return services;
    }
}