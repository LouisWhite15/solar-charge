using SolarCharge.API.Application.Features.Vehicles.Infrastructure;
using SolarCharge.API.Infrastructure.Tesla;

namespace SolarCharge.API.Application.Features.Vehicles;

public static class VehicleModule
{
    public static IServiceCollection AddVehicle(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TeslaOptions>(
            configuration.GetSection(TeslaOptions.SectionName));

        services.AddTransient<ITeslaClient, TeslaClient>();

        return services;
    }
}