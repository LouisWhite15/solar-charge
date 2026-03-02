using SolarCharge.API.Application.Features.ChargingStrategy.Services;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles.Models;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Commands;

public sealed record ExecuteChargingStrategyCommand(VehicleDto Vehicle, InverterTelemetryResult InverterTelemetryResult)
{
    public class Handler(
        ILogger<Handler> logger,
        IEnumerable<IChargingStrategy> chargingStrategies)
        : IWolverineHandler
    {
        public async Task HandleAsync(ExecuteChargingStrategyCommand command, CancellationToken cancellationToken = default)
        {
            var strategy = chargingStrategies.FirstOrDefault(strategy => strategy.CanEvaluate(command));
            if (strategy is null)
            {
                logger.LogWarning("No charging strategy could be found to evaluate the current state of the vehicle. VehicleId: {VehicleId}. IsCharging: {IsCharging}",
                    command.Vehicle.Id,
                    command.Vehicle.IsCharging);
                return;
            }
            
            logger.LogDebug("Executing charging strategy '{ChargingStrategyType}'. VehicleId: {VehicleId}. IsCharging: {IsCharging}",
                strategy.GetType().Name,
                command.Vehicle.Id,
                command.Vehicle.IsCharging);
            
            await strategy.EvaluateAsync(command.InverterTelemetryResult, cancellationToken);
        }
    }
}