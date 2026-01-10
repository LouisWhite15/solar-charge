using Microsoft.Extensions.Options;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.TeslaAuth.Commands;
using Wolverine;

namespace SolarCharge.API.Infrastructure.HostedServices;

public class RefreshTeslaAccessTokenHostedService(
    ILogger<RefreshTeslaAccessTokenHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<ApplicationOptions> applicationOptions) 
    : AsyncTimedHostedService(logger, applicationOptions.Value.RefreshTeslaAccessTokenFrequencySeconds)
{
    protected override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Refresh token job started");
        
        var scope = serviceScopeFactory.CreateScope();
        var messageBus = scope.ServiceProvider.GetRequiredService<IMessageBus>();
        
        var refreshCommand = new RefreshTeslaCommand();
        await messageBus.InvokeAsync(refreshCommand, cancellationToken);
        
        logger.LogTrace("Refresh token job completed");
    }
}