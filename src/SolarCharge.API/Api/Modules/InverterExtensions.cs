using SolarCharge.API.Application.Ports;
using SolarCharge.API.Infrastructure.Inverter;
using SolarCharge.API.Infrastructure.Inverter.Fronius;

namespace SolarCharge.API.Api.Modules;

public static class InverterExtensions
{
    public static IServiceCollection AddInverter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InverterOptions>(
            configuration.GetSection(InverterOptions.Inverter));
        
        services.AddTransient<IInverter, InverterService>();
        
        services.AddKeyedTransient<IInverter, NoOpInverterService>(InverterType.Unknown);
        services.AddKeyedTransient<IInverter, FroniusInverterService>(InverterType.Fronius);

        return services;
    }
}