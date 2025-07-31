using Coravel.Invocable;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Application.Invokables;

public class EvaluateSolarGenerationInvokable(
    ILogger<EvaluateSolarGenerationInvokable> logger,
    IInfluxDb influxDb,
    IServiceProvider serviceProvider)
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

        // We will retrieve the known vehicle state here later but for now we don't know
        const ChargeState chargeState = ChargeState.Unknown;
        var chargingStrategy = serviceProvider.GetRequiredKeyedService<IChargingStrategy>(chargeState);

        await chargingStrategy.Evaluate(influxInverterStatusResult);
    }
}