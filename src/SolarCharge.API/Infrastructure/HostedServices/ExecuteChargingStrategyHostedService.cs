using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Features.Vehicles.Queries;
using SolarCharge.API.Application.Features.Vehicles.UpdateVehicleStateFromTesla;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Services.ChargingStrategies;
using Wolverine;

namespace SolarCharge.API.Application.HostedServices;

public class ExecuteChargingStrategyHostedService(
    ILogger<ExecuteChargingStrategyHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IInfluxDb influxDb,
    IOptions<ApplicationOptions> applicationOptions,
    IMessageBus bus)
    : AsyncTimedHostedService(logger, applicationOptions.Value.EvaluateSolarGenerationFrequencySeconds)
{
    protected override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Evaluating solar generation");
        
        // Retrieve solar generation values from InfluxDB
        const string query = """
                                from(bucket: "home")
                                  |> range(start: -20m)
                                  |> filter(fn: (r) => r._measurement == "grid" or r._measurement == "pv" or r._measurement == "load")
                                  |> filter(fn: (r) => r._field == "value")
                                  |> pivot(rowKey:["_time"], columnKey: ["_measurement"], valueColumn: "_value")
                             """;
        
        var tables = await influxDb.QueryAsync(queryApi => 
            queryApi.QueryAsync(query, "solar-charge"));
        
        var records = tables.SelectMany(t => t.Records).ToList();
        if (records.Count == 0)
        {
            logger.LogWarning("No records found for inverter status. Not enough data to evaluate a charging strategy");
            return;
        }
        
        var influxInverterStatusResult = new InverterStatusResult(records);
        
        // Retrieve vehicle from persistence
        var vehicle = await bus.InvokeAsync<VehicleDto?>(new GetVehicleQuery(), cancellationToken);
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        // Execute command to retrieve and update the vehicle state
        logger.LogInformation("Sending {CommandName}", nameof(UpdateVehicleStateFromTeslaCommand));
        await bus.InvokeAsync(new UpdateVehicleStateFromTeslaCommand(vehicle.Id), cancellationToken);
        
        // Retrieve the updated state of the vehicle
        vehicle = await bus.InvokeAsync<VehicleDto?>(new GetVehicleQuery(), cancellationToken);
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        // Execute the charging strategy for the state of the vehicle
        using var scope = serviceScopeFactory.CreateScope();
        var chargingStrategy = scope.ServiceProvider.GetRequiredKeyedService<IChargingStrategy>(vehicle.State);
        
        logger.LogDebug("Executing charging strategy. State: {ChargeState}. VehicleId: {VehicleId}", vehicle.State, vehicle.Id);
        await chargingStrategy.Evaluate(influxInverterStatusResult, vehicle);
    }
}