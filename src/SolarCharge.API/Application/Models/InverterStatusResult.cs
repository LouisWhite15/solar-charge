using InfluxDB.Client.Core.Flux.Domain;

namespace SolarCharge.API.Application.Models;

public sealed record InverterStatusResult(Dictionary<DateTime, InverterStatus> Result)
{
    public InverterStatusResult(IEnumerable<FluxRecord> records)
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