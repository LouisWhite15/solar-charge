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
        
        logger.LogInformation("Sending {CommandName}",  nameof(UpdateStateCommand));
        await bus.InvokeAsync(new UpdateStateCommand(vehicle.Id));
    }
}