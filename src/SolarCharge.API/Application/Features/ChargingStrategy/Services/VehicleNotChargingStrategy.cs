using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.ChargingStrategy.Commands;
using SolarCharge.API.Application.Features.ChargingStrategy.Events;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles.Models;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public class VehicleNotChargingStrategy(
    ILogger<VehicleNotChargingStrategy> logger,
    IOptions<ChargingStrategyOptions> chargingStrategyOptions,
    IMessageBus messageBus)
    : IChargingStrategy
{
    public bool CanEvaluate(ExecuteChargingStrategyCommand command)
    {
        return command.Vehicle is { IsCharging: false, State: not VehicleStateDto.Unknown };
    }
    
    public async Task EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);

        var startChargingExcessGenerationThresholdWatts = chargingStrategyOptions.Value.StartChargingExcessGenerationThresholdWatts;
        
        var orderedInverterStatuses = inverterTelemetryResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        
        var gridAbsoluteWatts = Math.Abs(mostRecentStatus.Grid);
        
        // If we are supplying more to the grid (negative value) than the configured start charging threshold, we should start charging
        if (mostRecentStatus.Grid < 0 && gridAbsoluteWatts > startChargingExcessGenerationThresholdWatts)
        {
            logger.LogDebug("Supplying {GridValue}W to the grid. This exceeds the configured threshold of {Threshold}W",
                gridAbsoluteWatts,
                startChargingExcessGenerationThresholdWatts);

            await messageBus.InvokeAsync(new ChargingStrategyDeterminedStartChargingEvent(gridAbsoluteWatts), cancellationToken);
            return;
        }
        
        logger.LogDebug("Condition to start charging was not met. Value retrieved from inverter telemetry: {GridValue}W. Configured threshold: {Threshold}W",
            mostRecentStatus.Grid,
            -startChargingExcessGenerationThresholdWatts);
    }
}