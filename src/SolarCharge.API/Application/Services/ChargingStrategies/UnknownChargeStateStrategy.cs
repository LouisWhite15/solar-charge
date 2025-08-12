using SolarCharge.API.Application.Commands;
using SolarCharge.API.Application.Models;
using Wolverine;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class UnknownChargeStateStrategy(
    ILogger<UnknownChargeStateStrategy> logger,
    IMessageBus bus)
    : IChargingStrategy
{
    public async Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating UnknownChargeStateStrategy");
        
        var orderedInverterStatuses = inverterStatusResult.Result.OrderBy(s => s.Key).ToList();
        var mostRecentStatus = orderedInverterStatuses.Last().Value;

        if (orderedInverterStatuses.All(s => s.Value.Grid > 0))
        {
            logger.LogDebug("Pulling from the grid for the entire period. Most recent reading: {Grid}", mostRecentStatus.Grid);
        }
        
        if (orderedInverterStatuses.All(s => s.Value.Grid < 0))
        {
            logger.LogDebug("Supplying the grid for the entire period, Most recent reading: {Grid}", mostRecentStatus.Grid);
        }

        if (orderedInverterStatuses.First().Value.Photovoltaic < mostRecentStatus.Photovoltaic)
        {
            logger.LogDebug("PV generation is increasing for the period. Most recent reading: {PV}", mostRecentStatus.Photovoltaic);
        }
        
        logger.LogInformation("Sending {CommandName}",  nameof(UpdateStateCommand));
        await bus.InvokeAsync(new UpdateStateCommand(vehicle.Id));
    }
}