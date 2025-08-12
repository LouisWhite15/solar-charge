using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Repositories;
using SolarCharge.API.Domain.ValueObjects;
using Wolverine;

namespace SolarCharge.API.Application.Commands;

public class UpdateStateCommandHandler(
    ILogger<UpdateStateCommandHandler> logger,
    ITesla tesla,
    IVehicleRepository repository)
    : IWolverineHandler
{
    public async Task Handle(UpdateStateCommand command)
    {
        logger.LogTrace("Handling {CommandType}", nameof(UpdateStateCommand));
        
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
        var vehicleState = await tesla.GetVehicleStateAsync(new VehicleDto(vehicle));
        
        vehicle.SetState(vehicleState?.State.ToDomain() ?? VehicleState.Unknown);
        
        await repository.UpdateAsync(vehicle);
        await repository.UnitOfWork.SaveEntitiesAsync();
    }
}