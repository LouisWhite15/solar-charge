using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Repositories;
using Wolverine;

namespace SolarCharge.API.Application.Commands;

public class SetChargeStateCommandHandler(
    ILogger<SetChargeStateCommandHandler> logger,
    ITesla tesla,
    IVehicleRepository repository) : IWolverineHandler
{
    public async Task Handle(SetChargeStateCommand command, CancellationToken cancellationToken)
    {
        logger.LogTrace("Handling {CommandType}", nameof(SetChargeStateCommand));
        
        var vehicle = await repository.GetAsync();
        if (vehicle is null)
        {
            logger.LogError("No vehicle has been created with this application");
            return;
        }

        if (vehicle.Id != command.VehicleId)
        {
            logger.LogError("Vehicle not found. Id: {Id}", command.VehicleId);
            return;
        }
        
        logger.LogInformation("Setting ChargeState for Vehicle. Id: {Id}", command.VehicleId);
        var chargeState = await tesla.GetChargeStateAsync(command.VehicleId);
        
        vehicle.SetChargeState(chargeState.ToDomain());
        await repository.UpdateAsync(vehicle);
    }
}