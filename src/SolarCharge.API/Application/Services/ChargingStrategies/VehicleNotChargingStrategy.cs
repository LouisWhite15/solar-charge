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
        
        if (mostRecentStatus.Grid <= -startChargingExcessGenerationThresholdWatts)
        {
            var wattageSuppliedToGrid = Math.Abs(mostRecentStatus.Grid);
            logger.LogDebug("Supplying {GridValue}W to the grid. This exceeds the configured threshold of {ExcessGenerationThresholdWatts}W",
                wattageSuppliedToGrid,
                startChargingExcessGenerationThresholdWatts);
            
            await notificationService.SendAsync(NotificationType.StartCharging, wattageSuppliedToGrid);
        }
    }
}