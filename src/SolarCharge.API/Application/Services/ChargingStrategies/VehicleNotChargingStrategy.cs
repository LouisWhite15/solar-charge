using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public class VehicleNotChargingStrategy(ILogger<VehicleNotChargingStrategy> logger)
    : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult, VehicleDto vehicle)
    {
        logger.LogInformation("Evaluating VehicleNotChargingStrategy");

        return Task.CompletedTask;
    }
}