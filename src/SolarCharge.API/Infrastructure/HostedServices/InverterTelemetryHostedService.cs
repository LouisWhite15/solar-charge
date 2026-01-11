using Microsoft.Extensions.Options;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.Inverter.Commands;
using Wolverine;

namespace SolarCharge.API.Infrastructure.HostedServices;

public class InverterTelemetryHostedService(
    ILogger<InverterTelemetryHostedService> logger,
    IOptions<ApplicationOptions> applicationOptions,
    IServiceScopeFactory serviceScopeFactory)
    : AsyncTimedHostedService(logger, applicationOptions.Value.InverterStatusCheckFrequencySeconds)
{
    protected override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
        
        await commandBus.InvokeAsync(new CollectInverterTelemetryCommand(), cancellationToken);
    }
}