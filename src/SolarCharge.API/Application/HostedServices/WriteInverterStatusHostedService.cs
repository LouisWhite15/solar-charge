using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Application.HostedServices;

public class WriteInverterStatusHostedService(
    ILogger<WriteInverterStatusHostedService> logger,
    IInfluxDb influxDb,
    IInverter inverter,
    IOptions<ApplicationOptions> applicationOptions)
    : AsyncTimedHostedService(logger, applicationOptions.Value.InverterStatusCheckFrequencySeconds)
{
    protected override async Task DoWorkAsync()
    {
        logger.LogDebug("Retrieving inverter status");
        var inverterStatus = await inverter.GetAsync();
        
        logger.LogDebug("Writing inverter status to InfluxDB");
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