using Microsoft.Extensions.Options;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.TeslaAuth.Commands;
using Wolverine;

namespace SolarCharge.API.Infrastructure.HostedServices;

public class RefreshTeslaAccessTokenHostedService(
    ILogger<RefreshTeslaAccessTokenHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<InfrastructureOptions> applicationOptions) 
    : AsyncTimedHostedService(logger, applicationOptions.Value.RefreshTeslaAccessTokenFrequencySeconds)
{
    protected override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Refresh token job started");
        
        var scope = serviceScopeFactory.CreateScope();
        var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
        
        var refreshCommand = new RefreshTeslaCommand();
        await commandBus.InvokeAsync(refreshCommand, cancellationToken);
        
        logger.LogTrace("Refresh token job completed");
    }
}