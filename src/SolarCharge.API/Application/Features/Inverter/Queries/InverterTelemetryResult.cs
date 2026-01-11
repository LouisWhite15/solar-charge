using InfluxDB.Client.Core.Flux.Domain;
using SolarCharge.API.Application.Features.Inverter.Domain;

namespace SolarCharge.API.Application.Features.Inverter.Queries;

public sealed record InverterTelemetryResult(Dictionary<DateTime, InverterStatus> Result)
{
    public InverterTelemetryResult(IEnumerable<FluxRecord> records)
        : this(new Dictionary<DateTime, InverterStatus>())
    {
        foreach (var record in records)
        {
            var pv = Convert.ToDouble(record.GetValueByKey("pv"));
            var grid = Convert.ToDouble(record.GetValueByKey("grid"));
            var load = Convert.ToDouble(record.GetValueByKey("load"));
            var inverterStatus = new InverterStatus(pv, grid, load);
			
            Result.Add(record.GetTimeInDateTime() ?? DateTime.MinValue, inverterStatus);
        }
    }
}