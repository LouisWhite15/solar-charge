using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using Wolverine;

namespace SolarCharge.API.Application.Features.Inverter.Commands;

public sealed record CollectInverterTelemetryCommand
{
    public class Handler(
        ILogger<Handler> logger,
        IInverterClient inverterClient,
        IInverterTelemetryRepository inverterTelemetryRepository)
        : IWolverineHandler
    {
        public async ValueTask HandleAsync(CollectInverterTelemetryCommand _, CancellationToken cancellationToken)
        {
            logger.LogDebug("Retrieving inverter status");
            var inverterStatus = await inverterClient.GetAsync(cancellationToken);
        
            logger.LogDebug("Persisting inverter status");
            await inverterTelemetryRepository.WriteAsync(inverterStatus, cancellationToken);
        }
    }
}