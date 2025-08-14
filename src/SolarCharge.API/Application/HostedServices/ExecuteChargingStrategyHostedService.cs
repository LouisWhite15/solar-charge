using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Queries;
using SolarCharge.API.Application.Services.ChargingStrategies;

namespace SolarCharge.API.Application.HostedServices;

public class ExecuteChargingStrategyHostedService(
    ILogger<ExecuteChargingStrategyHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IServiceProvider serviceProvider,
    IInfluxDb influxDb,
    IOptions<ApplicationOptions> applicationOptions)
    : AsyncTimedHostedService(logger, applicationOptions.Value.EvaluateSolarGenerationFrequencySeconds)
{
    protected override async Task DoWorkAsync()
    {
        logger.LogDebug("Evaluating solar generation");
        
        const string query = """
                                from(bucket: "home")
                                  |> range(start: -20m)
                                  |> filter(fn: (r) => r._measurement == "grid" or r._measurement == "pv" or r._measurement == "load")
                                  |> filter(fn: (r) => r._field == "value")
                                  |> pivot(rowKey:["_time"], columnKey: ["_measurement"], valueColumn: "_value")
                             """;
        
        var tables = await influxDb.QueryAsync(queryApi => 
            queryApi.QueryAsync(query, "solar-charge"));
        
        var records = tables.SelectMany(t => t.Records);
        var influxInverterStatusResult = new InverterStatusResult(records);

        using var scope = serviceScopeFactory.CreateScope();
        var vehicleQueries = scope.ServiceProvider.GetRequiredService<IVehicleQueries>();
        
        var vehicle = await vehicleQueries.GetVehicleAsync();
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle not found. No charging strategy will be executed");
            return;
        }
        
        var chargingStrategy = serviceProvider.GetRequiredKeyedService<IChargingStrategy>(vehicle.State);
        
        logger.LogDebug("Executing charging strategy. State: {ChargeState}. VehicleId: {VehicleId}", vehicle.State, vehicle.Id);
        await chargingStrategy.Evaluate(influxInverterStatusResult, vehicle);
    }
}