using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using SolarCharge.API.Application.Features.TeslaAuth.Services;
using SolarCharge.API.Infrastructure.Tesla;

namespace SolarCharge.API.Application.Features.TeslaAuth;

public static class TeslaModule
{
    public static IServiceCollection AddTeslaAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TeslaOptions>(
            configuration.GetSection(TeslaOptions.SectionName));
        
        services.AddTransient<ITeslaAuthenticationService, TeslaAuthenticationService>();
        
        services.AddScoped<ITeslaAuthenticationRepository, TeslaAuthenticationRepository>();
        services.AddTransient<ITeslaAuthenticationClient, TeslaAuthenticationClient>();

        return services;
    }
}