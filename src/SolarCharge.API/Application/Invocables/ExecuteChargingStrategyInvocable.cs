using Coravel.Invocable;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Queries;
using SolarCharge.API.Application.Services.ChargingStrategies;

namespace SolarCharge.API.Application.Invocables;

public class ExecuteChargingStrategyInvocable(
    ILogger<ExecuteChargingStrategyInvocable> logger,
    IInfluxDb influxDb,
    IServiceProvider serviceProvider,
    IVehicleQueries vehicleQueries)
    : IInvocable
{
    public async Task Invoke()
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

        var vehicle = await vehicleQueries.GetVehicleAsync();
        var chargeState = vehicle?.ChargeState ?? ChargeStateDto.Unknown;
        
        var chargingStrategy = serviceProvider.GetRequiredKeyedService<IChargingStrategy>(chargeState);
        
        logger.LogDebug("Executing charging strategy for {ChargeState}", chargeState);
        await chargingStrategy.Evaluate(influxInverterStatusResult);
    }
}