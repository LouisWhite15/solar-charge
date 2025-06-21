using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Infrastructure.Inverter;

namespace SolarCharge.API.Application.Invokables;

public class WriteInverterStatusInvokable(
    ILogger<WriteInverterStatusInvokable> logger,
    IOptions<InverterOptions> inverterOptions,
    IServiceProvider serviceProvider,
    IInfluxDb influxDb) 
    : Coravel.Invocable.IInvocable
{
    private readonly IInverter _inverter = serviceProvider.GetRequiredKeyedService<IInverter>(inverterOptions.Value.Type);

    public async Task Invoke()
    {
        logger.LogTrace("Retrieving inverter status");
        var inverterStatus = await _inverter.GetAsync();

        logger.LogTrace("Writing inverter status to InfluxDB");
        influxDb.Write(write =>
        {
            var now = DateTime.UtcNow;
            
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
        });
        
    }
}