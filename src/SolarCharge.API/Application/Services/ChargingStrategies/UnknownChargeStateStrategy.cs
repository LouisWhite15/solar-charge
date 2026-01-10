using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Models;
using Wolverine;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class UnknownChargeStateStrategy(
    ILogger<UnknownChargeStateStrategy> logger,
    IMessageBus bus)
    : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating {Strategy}", GetType().Name);
        
        var orderedInverterStatuses = inverterStatusResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;
        
        logger.LogTrace("Most recent reading: Grid: {Grid}W. PV: {PV}W", mostRecentStatus.Grid, mostRecentStatus.Photovoltaic);
        return Task.CompletedTask;
    }
}