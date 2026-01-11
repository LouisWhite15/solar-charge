using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public class VehicleChargingStrategy(
    ILogger<VehicleChargingStrategy> logger)
    : IChargingStrategy
{
    public ValueTask EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default)
    {
        // This charging strategy is currently unused as we are not current retrieving charge state
        // This will be implemented soon
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);
        
        var orderedInverterStatuses = inverterTelemetryResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        return ValueTask.CompletedTask;
    }
}