using MediatR;
using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.Repositories;

namespace SolarCharge.API.Application.Commands;

public class CreateVehicleCommandHandler(
    ILogger<CreateVehicleCommandHandler> logger,
    ITesla tesla,
    IVehicleRepository repository) : IRequestHandler<CreateVehicleCommand>
{
    public async Task Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        var vehicleDetails = await tesla.GetVehicleAsync();
        if (vehicleDetails is null)
        {
            logger.LogError("Vehicle could not be retrieved from Telsa");
            return;
        }

        var chargeState = await tesla.GetChargeStateAsync(vehicleDetails.Id);
        if (chargeState is ChargeStateDto.Unknown)
        {
            logger.LogWarning("Could not retrieve charge state for Vehicle. Id: {Id}. DisplayName: {DisplayName}",
                vehicleDetails.Id,
                vehicleDetails.DisplayName);
            return;
        }

        var existingVehicle = await repository.GetAsync();
        if (existingVehicle is not null && existingVehicle.Id != vehicleDetails.Id)
        {
            logger.LogInformation("Another vehicle already exists. Removing and adding new vehicle. Existing Id: {ExistingId}. New Id: {NewId}",
                existingVehicle.Id,
                vehicleDetails.Id);
            await repository.DeleteAsync(existingVehicle.Id);
        }

        var vehicle = new Vehicle(vehicleDetails.Id, vehicleDetails.DisplayName, chargeState.ToDomain());
        await repository.AddAsync(vehicle);
    }
}