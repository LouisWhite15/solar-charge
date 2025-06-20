using SolarCharge.API.Application.Services;
using SolarCharge.API.Infrastructure.Inverter;

namespace SolarCharge.API.WebApi.Modules;

public static class InverterExtensions
{
    public static IServiceCollection AddInverter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InverterOptions>(
            configuration.GetSection(InverterOptions.Inverter));
        
        services.AddHttpClient();
        services.AddKeyedScoped<IInverter, NoOpInverterService>(InverterType.Unknown);
        services.AddKeyedScoped<IInverter, FroniusInverterService>(InverterType.Fronius);

        return services;
    }
}