using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Features.Vehicles.Extensions;
using SolarCharge.API.Application.Features.Vehicles.Infrastructure;
using SolarCharge.API.Application.Shared;
using SolarCharge.API.Infrastructure.Database;
using Wolverine;

namespace SolarCharge.API.Application.Features.Vehicles.Commands;

public sealed record UpdateVehicleStateFromTeslaCommand(long VehicleId)
{
    public class Handler(
        ILogger<Handler> logger,
        ITeslaClient teslaClient,
        ApplicationDbContext dbContext,
        IClock clock)
        : IWolverineHandler
    {
        public async Task HandleAsync(UpdateVehicleStateFromTeslaCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogTrace("Handling {CommandType}", nameof(UpdateVehicleStateFromTeslaCommand));
        
            var vehicle = await dbContext.Vehicles.FirstOrDefaultAsync(cancellationToken);
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
        
            logger.LogInformation("Retrieving vehicle state for vehicle. Id: {Id}", command.VehicleId);
            var vehicleState = await teslaClient.GetVehicleStateAsync(vehicle.Id, cancellationToken);

            if (vehicleState is null)
            {
                logger.LogWarning("Could not update vehicle state from Tesla. Id: {Id}", command.VehicleId);
                return;
            }

            var vehicleTelemetry = new VehicleTelemetry(
                vehicleState.State.ToDomain(),
                vehicleState.IsCharging,
                clock.Now);

            vehicle.ApplyTelemetry(vehicleTelemetry);
            
            await dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}
