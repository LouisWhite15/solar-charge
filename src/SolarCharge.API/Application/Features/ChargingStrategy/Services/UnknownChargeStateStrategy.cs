using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public class UnknownChargeStateStrategy(
    ILogger<UnknownChargeStateStrategy> logger)
    : IChargingStrategy
{
    public ValueTask EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);
        
        var orderedInverterStatuses = inverterTelemetryResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        return ValueTask.CompletedTask;
    }
}