using Microsoft.Extensions.Options;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Features.ChargingStrategy.Commands;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Features.Vehicles.Commands;
using SolarCharge.API.Application.Features.Vehicles.Queries;
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
        
        using var scope = serviceScopeFactory.CreateScope();
        var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
        
        // Retrieve solar generation values
        var inverterTelemetryResult = await commandBus.InvokeAsync<InverterTelemetryResult>(new SearchInverterTelemetryQuery(TimeSpan.FromMinutes(-20)), cancellationToken);
        if (inverterTelemetryResult.Result.Count == 0)
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
        
        // Update vehicle state from tesla
        logger.LogInformation("Sending {CommandName}", nameof(UpdateVehicleStateFromTeslaCommand));
        await commandBus.InvokeAsync(new UpdateVehicleStateFromTeslaCommand(vehicle.Id), cancellationToken);
        
        // Retrieve the updated state of the vehicle
        vehicle = await commandBus.InvokeAsync<VehicleDto?>(new GetVehicleQuery(), cancellationToken);
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        // Execute the charging strategy
        var executeChargingStrategyCommand = new ExecuteChargingStrategyCommand(vehicle, inverterTelemetryResult);
        await commandBus.InvokeAsync(executeChargingStrategyCommand, cancellationToken);
    }
}