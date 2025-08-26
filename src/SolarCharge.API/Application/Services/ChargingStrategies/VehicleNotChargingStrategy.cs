using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class VehicleNotChargingStrategy(
    ILogger<VehicleNotChargingStrategy> logger,
    IOptions<ApplicationOptions> applicationOptions,
    INotificationService notificationService)
    : IChargingStrategy
{
    public async Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating VehicleNotChargingStrategy");

        var startChargingExcessGenerationThresholdWatts = applicationOptions.Value.StartChargingExcessGenerationThresholdWatts;
        
        var orderedInverterStatuses = inverterStatusResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        
        var gridAbsoluteWatts = Math.Abs(mostRecentStatus.Grid);
        if (mostRecentStatus.Grid <= -startChargingExcessGenerationThresholdWatts)
        {
            logger.LogDebug("Supplying {GridValue}W to the grid. This exceeds the configured threshold of {ExcessGenerationThresholdWatts}W",
                gridAbsoluteWatts,
                startChargingExcessGenerationThresholdWatts);
            
            await notificationService.SendAsync(NotificationType.StartCharging, gridAbsoluteWatts);
        }
        else if (mostRecentStatus.Grid < 0)
        {
            logger.LogDebug("Not supplying enough to the grid to start charging. Supplying {SupplyingWatts}W. Threshold: {Threshold}W",
                gridAbsoluteWatts,
                startChargingExcessGenerationThresholdWatts);
            await notificationService.SendAsync(NotificationType.StopCharging, gridAbsoluteWatts);
        }
        else
        {
            logger.LogDebug("Pulling from the grid. Pulling: {PullingWatts}W", gridAbsoluteWatts);
            await notificationService.SendAsync(NotificationType.StopCharging, gridAbsoluteWatts);
        }
    }
}