using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class VehicleChargingStrategy(
    ILogger<VehicleChargingStrategy> logger,
    IOptions<ApplicationOptions> applicationOptions,
    INotificationService notificationService)
    : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        // This charging strategy is currently unused as we are not current retrieving charge state
        // This will be implemented soon
        logger.LogInformation("Evaluating UnknownChargeStateStrategy");
        
        var orderedInverterStatuses = inverterStatusResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        return Task.CompletedTask;
    }
}