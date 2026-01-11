using SolarCharge.API.Application.Features.ChargingStrategy.Services;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Commands;

public sealed record ExecuteChargingStrategyCommand(VehicleDto Vehicle, InverterTelemetryResult InverterTelemetryResult)
{
    public class Handler(
        ILogger<Handler> logger,
        IServiceProvider serviceProvider)
        : IWolverineHandler
    {
        public async ValueTask HandleAsync(ExecuteChargingStrategyCommand command, CancellationToken cancellationToken = default)
        {
            var scope = serviceProvider.CreateScope();
            var chargingStrategy = scope.ServiceProvider.GetRequiredKeyedService<IChargingStrategy>(command.Vehicle.State);
        
            logger.LogDebug("Executing charging strategy. State: {ChargeState}. VehicleId: {VehicleId}", command.Vehicle.State, command.Vehicle.Id);
            await chargingStrategy.EvaluateAsync(command.InverterTelemetryResult, cancellationToken);
        }
    }
}