using InfluxDB.Client.Core.Flux.Domain;
using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Application.Models;

public class InverterStatusResult
{
    public Dictionary<DateTime, InverterStatus> Result { get; } = new();
	
    public InverterStatusResult(IEnumerable<FluxRecord> records)
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