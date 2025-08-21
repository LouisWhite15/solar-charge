using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class VehicleChargingStrategy(
    ILogger<VehicleChargingStrategy> logger,
    IOptions<ApplicationOptions> applicationOptions,
    INotificationService notificationService)
    : IChargingStrategy
{
    public async Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating VehicleChargingStrategy");

        var stopChargingPullingFromGridThresholdWatts = applicationOptions.Value.StopChargingPullingFromGridThresholdWatts;
        
        var orderedInverterStatuses = inverterStatusResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses[-1].Value;
        
        if (mostRecentStatus.Grid >= stopChargingPullingFromGridThresholdWatts)
        {
            var wattagePullingFromGrid = Math.Abs(mostRecentStatus.Grid);
            logger.LogDebug("Pulling {GridValue}W from the grid. This exceeds the configured threshold of {PullingFromGridThreshold}W",
                wattagePullingFromGrid,
                stopChargingPullingFromGridThresholdWatts);
            
            await notificationService.SendAsync(NotificationType.StopCharging, wattagePullingFromGrid);
        }
    }
}