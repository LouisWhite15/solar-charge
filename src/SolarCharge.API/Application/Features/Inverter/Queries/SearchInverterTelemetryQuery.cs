using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using Wolverine;

namespace SolarCharge.API.Application.Features.Inverter.Queries;

public sealed record SearchInverterTelemetryQuery(TimeSpan TimeSpan)
{
    public class Handler(
        ILogger<SearchInverterTelemetryQuery> logger,
        IInverterTelemetryRepository repository) : IWolverineHandler
    {
        public async ValueTask<InverterTelemetryResult> HandleAsync(SearchInverterTelemetryQuery query, CancellationToken cancellationToken = default)
        {
            logger.LogTrace("Querying inverter status");
            
            return await repository.QueryAsync(query.TimeSpan, cancellationToken);
        }
    }
}