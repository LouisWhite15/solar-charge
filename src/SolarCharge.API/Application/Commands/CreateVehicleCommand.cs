using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.Repositories;
using SolarCharge.API.Domain.ValueObjects;
using Wolverine;

namespace SolarCharge.API.Application.Commands;

public sealed record CreateVehicleCommand
{
    public class Handler(
        ILogger<Handler> logger,
        ITesla tesla,
        IVehicleRepository repository)
        : IWolverineHandler
    {
        public async Task Handle(CreateVehicleCommand _)
        {
            logger.LogInformation("Handling {CommandType}", nameof(CreateVehicleCommand));
        
            var vehicleDetails = await tesla.GetVehicleAsync();
            if (vehicleDetails is null)
            {
                logger.LogError("Vehicle could not be retrieved from Tesla");
                return;
            }

            var vehicleState = await tesla.GetVehicleStateAsync(vehicleDetails);
            if (vehicleState is null)
            {
                logger.LogWarning("Could not retrieve state for Vehicle. Id: {Id}. DisplayName: {DisplayName}",
                    vehicleDetails.Id,
                    vehicleDetails.DisplayName);
            }

            var existingVehicle = await repository.GetAsync();
            if (existingVehicle is not null && existingVehicle.Id != vehicleDetails.Id)
            {
                logger.LogInformation("Another vehicle already exists. Removing existing vehicle. Existing Vehicle Id: {ExistingVehicleId}",
                    existingVehicle.Id);
                await repository.DeleteAsync(existingVehicle.Id);
            }

            logger.LogInformation("Creating vehicle. Id: {Id}. DisplayName: {DisplayName}",
                vehicleDetails.Id,
                vehicleDetails.DisplayName);
        
            var vehicle = new Vehicle(
                vehicleDetails.Id,
                vehicleDetails.DisplayName,
                vehicleState?.State.ToDomain() ?? VehicleState.Unknown);
        
            await repository.AddAsync(vehicle);
            await repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
