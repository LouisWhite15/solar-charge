using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class VehicleChargingStrategy(ILogger<VehicleChargingStrategy> logger)
    : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating VehicleChargingStrategy");

        return Task.CompletedTask;
    }
}