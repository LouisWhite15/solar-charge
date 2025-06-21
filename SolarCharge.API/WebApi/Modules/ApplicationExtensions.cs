using Coravel;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Invokables;

namespace SolarCharge.API.WebApi.Modules;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<ApplicationOptions>(
            configuration.GetSection(ApplicationOptions.Application));
        
        services.Configure<FeatureOptions>(
            configuration.GetSection(FeatureOptions.Features));
        
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        // Coravel scheduling
        services.AddTransient<WriteInverterStatusInvokable>();
        services.AddScheduler();
        
        return services;
    }
}