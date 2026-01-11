using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Shared;

namespace SolarCharge.API.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<FeatureOptions>(
            configuration.GetSection(FeatureOptions.SectionName));
        
        services.AddHttpClient();
        
        // Shared
        services.AddTransient<IClock, Clock>();
        
        return services;
    }
}