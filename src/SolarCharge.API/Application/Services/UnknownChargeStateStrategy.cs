using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services;

public class UnknownChargeStateStrategy(ILogger<UnknownChargeStateStrategy> logger) : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult)
    {
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

        return Task.CompletedTask;
    }
}