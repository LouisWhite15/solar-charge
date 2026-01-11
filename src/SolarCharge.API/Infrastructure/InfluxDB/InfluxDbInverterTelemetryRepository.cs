using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Shared;
using SolarCharge.API.Infrastructure.InfluxDB.Extensions;

namespace SolarCharge.API.Infrastructure.InfluxDB;

public class InfluxDbInverterTelemetryRepository(
    ILogger<InfluxDbInverterTelemetryRepository> logger,
    IOptions<InfluxDbOptions> influxDbOptions,
    IClock clock) 
    : IInverterTelemetryRepository
{
    private readonly string _url = influxDbOptions.Value.Url;
    private readonly string _token = influxDbOptions.Value.Token;
    
    public ValueTask WriteAsync(InverterStatus inverterStatus, CancellationToken cancellationToken = default)
    {
        using var client = new InfluxDBClient(_url, _token);
        using var write = client.GetWriteApi();

        var now = clock.UtcNow;
        var pvPoint = PointData.Measurement("pv")
            .Tag("inverter", "inverter-status")
            .Field("value", inverterStatus.Photovoltaic)
            .Timestamp(now, WritePrecision.Ms);
            
        var gridPoint = PointData.Measurement("grid")
            .Tag("inverter", "inverter-status")
            .Field("value", inverterStatus.Grid)
            .Timestamp(now, WritePrecision.Ms);
            
        var loadPoint = PointData.Measurement("load")
            .Tag("inverter", "inverter-status")
            .Field("value", inverterStatus.Load)
            .Timestamp(now, WritePrecision.Ms);

        write.WritePoints(new [] { pvPoint, gridPoint, loadPoint }, "home", "solar-charge");
        
        return ValueTask.CompletedTask;
    }

    public async ValueTask<InverterTelemetryResult> QueryAsync(TimeSpan timeSpan, CancellationToken cancellationToken = default)
    {
        using var client = new InfluxDBClient(_url, _token);
        var queryApi = client.GetQueryApi();
        
        var query = $"""
                        from(bucket: "home")
                          |> range(start: {timeSpan.ToInfluxQueryRange()})
                          |> filter(fn: (r) => r._measurement == "grid" or r._measurement == "pv" or r._measurement == "load")
                          |> filter(fn: (r) => r._field == "value")
                          |> pivot(rowKey:["_time"], columnKey: ["_measurement"], valueColumn: "_value")
                    """;
        
        logger.LogTrace("InfluxDb query: {NewLine}{Query}", Environment.NewLine, query);
        
        var tables = await queryApi.QueryAsync(query, "solar-charge", cancellationToken);
        
        var records = tables.SelectMany(t => t.Records).ToList();
        if (records.Count == 0)
        {
            logger.LogWarning("No records found for inverter status");
        }
        
        return new InverterTelemetryResult(records);
    }
}