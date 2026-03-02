using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.ChargingStrategy.Commands;
using SolarCharge.API.Application.Features.ChargingStrategy.Events;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles.Models;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public class VehicleChargingStrategy(
    ILogger<VehicleChargingStrategy> logger,
    IOptions<ChargingStrategyOptions> chargingStrategyOptions,
    IMessageBus messageBus)
    : IChargingStrategy
{
    public bool CanEvaluate(ExecuteChargingStrategyCommand command)
    {
        return command.Vehicle is { IsCharging: true, State: not VehicleStateDto.Unknown };
    }

    public async Task EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);
        
        var stopChargingPullingFromGridThresholdWatts = chargingStrategyOptions.Value.StopChargingPullingFromGridThresholdWatts;
        
        var orderedInverterStatuses = inverterTelemetryResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        
        var gridAbsoluteWatts = Math.Abs(mostRecentStatus.Grid);
        
        // If we are pulling more than the configured threshold to stop charging to the grid, we should stop charging
        if (mostRecentStatus.Grid > stopChargingPullingFromGridThresholdWatts)
        {
            logger.LogDebug("Pulling {GridValue}W from the grid. This exceeds the configured threshold of {Threshold}W",
                gridAbsoluteWatts,
                stopChargingPullingFromGridThresholdWatts);
            
            await messageBus.InvokeAsync(new ChargingStrategyDeterminedStopChargingEvent(gridAbsoluteWatts), cancellationToken);
            return;
        }
        
        logger.LogDebug("Condition to stop charging was not met. Value retrieved from inverter telemetry: {GridValue}W. Configured threshold: {Threshold}W",
            mostRecentStatus.Grid,
            stopChargingPullingFromGridThresholdWatts);
    }
}