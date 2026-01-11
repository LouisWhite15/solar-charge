using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.ChargingStrategy.Events;
using SolarCharge.API.Application.Features.Inverter.Queries;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public class VehicleNotChargingStrategy(
    ILogger<VehicleNotChargingStrategy> logger,
    IOptions<ChargingStrategyOptions> chargingStrategyOptions,
    IMessageBus messageBus)
    : IChargingStrategy
{
    public async ValueTask EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);

        var startChargingExcessGenerationThresholdWatts = chargingStrategyOptions.Value.StartChargingExcessGenerationThresholdWatts;
        var stopChargingPullingFromGridThresholdWatts = chargingStrategyOptions.Value.StopChargingPullingFromGridThresholdWatts;
        
        var orderedInverterStatuses = inverterTelemetryResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        
        var gridAbsoluteWatts = Math.Abs(mostRecentStatus.Grid);
        
        // This is pretty bad
        // The point of these strategies is to avoid this kind of mess
        // but while we don't have the proper charging state coming through this will have to do
        
        // If we are supplying more to the grid than the configured start charging threshold, we should start charging
        if (mostRecentStatus.Grid <= -startChargingExcessGenerationThresholdWatts)
        {
            logger.LogDebug("Supplying {GridValue}W to the grid. This exceeds the configured threshold of {Threshold}W",
                gridAbsoluteWatts,
                startChargingExcessGenerationThresholdWatts);

            await messageBus.InvokeAsync(new ChargingStrategyDeterminedStartChargingEvent(gridAbsoluteWatts), cancellationToken);
        }
        // If we are pulling more than the configured threshold to stop charging to the grid, we should stop charging
        else if (mostRecentStatus.Grid > stopChargingPullingFromGridThresholdWatts)
        {
            logger.LogDebug("Pulling {GridValue}W from the grid. This exceeds the configured threshold of {Threshold}W",
                gridAbsoluteWatts,
                stopChargingPullingFromGridThresholdWatts);
            
            await messageBus.InvokeAsync(new ChargingStrategyDeterminedStopChargingEvent(gridAbsoluteWatts), cancellationToken);
        }
        // Do a few more checks for logging purposes
        else switch (mostRecentStatus.Grid)
        {
            
            case < 0:
                logger.LogDebug("Supplying {GridValue}W to the grid. Not enough to trigger the start charge condition", gridAbsoluteWatts);
                break;
            case > 0:
                logger.LogDebug("Pulling {GridValue}W from the grid. Not enough to trigger the stop charge condition", gridAbsoluteWatts);
                break;
        }
    }
}