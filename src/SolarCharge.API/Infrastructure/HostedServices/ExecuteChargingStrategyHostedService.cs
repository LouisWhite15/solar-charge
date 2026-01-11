using Microsoft.Extensions.Options;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Features.Vehicles.Queries;
using SolarCharge.API.Application.Features.Vehicles.UpdateVehicleStateFromTesla;
using SolarCharge.API.Application.Services.ChargingStrategies;
using Wolverine;

namespace SolarCharge.API.Infrastructure.HostedServices;

public class ExecuteChargingStrategyHostedService(
    ILogger<ExecuteChargingStrategyHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<ApplicationOptions> applicationOptions)
    : AsyncTimedHostedService(logger, applicationOptions.Value.EvaluateSolarGenerationFrequencySeconds)
{
    protected override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Evaluating solar generation");
        
        // Retrieve solar generation values
        using var scope = serviceScopeFactory.CreateScope();
        var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
        
        var influxInverterStatusResult = await commandBus.InvokeAsync<InverterTelemetryResult>(new SearchInverterTelemetryQuery(TimeSpan.FromMinutes(-20)), cancellationToken);
        if (influxInverterStatusResult.Result.Count == 0)
        {
            logger.LogWarning("No inverter telemetry found. Not enough data to evaluate a charging strategy");
            return;
        }
        
        // Retrieve vehicle
        var vehicle = await commandBus.InvokeAsync<VehicleDto?>(new GetVehicleQuery(), cancellationToken);
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        // Update state from Tesla
        logger.LogInformation("Sending {CommandName}", nameof(UpdateVehicleStateFromTeslaCommand));
        await commandBus.InvokeAsync(new UpdateVehicleStateFromTeslaCommand(vehicle.Id), cancellationToken);
        
        // Retrieve the updated state of the vehicle
        vehicle = await commandBus.InvokeAsync<VehicleDto?>(new GetVehicleQuery(), cancellationToken);
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        // Execute the charging strategy for the state of the vehicle
        var chargingStrategy = scope.ServiceProvider.GetRequiredKeyedService<IChargingStrategy>(vehicle.State);
        
        logger.LogDebug("Executing charging strategy. State: {ChargeState}. VehicleId: {VehicleId}", vehicle.State, vehicle.Id);
        await chargingStrategy.Evaluate(influxInverterStatusResult, vehicle);
    }
}