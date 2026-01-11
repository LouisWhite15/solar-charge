using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Features.Vehicles.Infrastructure;
using SolarCharge.API.Application.Shared.Extensions;
using SolarCharge.API.Infrastructure.DataAccess;
using Wolverine;

namespace SolarCharge.API.Application.Features.Vehicles.Commands;

public sealed record RegisterVehicleFromTeslaCommand
{
    public class Handler(
        ILogger<Handler> logger,
        ITeslaClient teslaClient,
        ApplicationDbContext dbContext)
        : IWolverineHandler
    {
        public async ValueTask HandleAsync(RegisterVehicleFromTeslaCommand _, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {CommandType}", nameof(RegisterVehicleFromTeslaCommand));
        
            var vehicleDetails = await teslaClient.GetVehicleAsync(cancellationToken);
            if (vehicleDetails is null)
            {
                logger.LogError("Vehicle could not be retrieved from Tesla");
                return;
            }

            var vehicleState = await teslaClient.GetVehicleStateAsync(vehicleDetails.Id, cancellationToken);
            if (vehicleState is null)
            {
                logger.LogWarning("Could not retrieve state for Vehicle. Id: {Id}. DisplayName: {DisplayName}",
                    vehicleDetails.Id,
                    vehicleDetails.DisplayName);
            }

            var existingVehicle = await dbContext.Vehicles.FirstOrDefaultAsync(cancellationToken);
            if (existingVehicle is not null && 
                existingVehicle.Id != vehicleDetails.Id)
            {
                logger.LogInformation("Another vehicle already exists. Removing existing vehicle. Existing Vehicle Id: {ExistingVehicleId}",
                    existingVehicle.Id);
                dbContext.Vehicles.Remove(existingVehicle);
            }

            logger.LogInformation("Creating vehicle. Id: {Id}. DisplayName: {DisplayName}",
                vehicleDetails.Id,
                vehicleDetails.DisplayName);
        
            var vehicle = new Vehicle(
                vehicleDetails.Id,
                vehicleDetails.DisplayName,
                vehicleState?.State.ToDomain() ?? VehicleState.Unknown);
        
            await dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
            await dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}
