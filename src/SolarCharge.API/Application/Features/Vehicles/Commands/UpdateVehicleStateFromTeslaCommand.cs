using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Features.Vehicles.Infrastructure;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Domain.ValueObjects;
using SolarCharge.API.Infrastructure.DataAccess;
using Wolverine;

namespace SolarCharge.API.Application.Features.Vehicles.UpdateVehicleStateFromTesla;

public sealed record UpdateVehicleStateFromTeslaCommand(long VehicleId)
{
    public class Handler(
        ILogger<Handler> logger,
        ITeslaClient teslaClient,
        ApplicationDbContext dbContext)
        : IWolverineHandler
    {
        public async ValueTask HandleAsync(UpdateVehicleStateFromTeslaCommand command, CancellationToken cancellationToken = default)
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
        
            logger.LogInformation("Setting ChargeState for Vehicle. Id: {Id}", command.VehicleId);
            var vehicleState = await teslaClient.GetVehicleStateAsync(vehicle.Id, cancellationToken);
        
            vehicle.SetState(vehicleState?.State.ToDomain() ?? VehicleState.Unknown);
        
            dbContext.Vehicles.Update(vehicle);
            await dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}
