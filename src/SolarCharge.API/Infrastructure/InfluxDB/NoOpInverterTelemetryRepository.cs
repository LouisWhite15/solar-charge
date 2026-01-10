using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Infrastructure.InfluxDB;

public class NoOpInverterTelemetryRepository : IInverterTelemetryRepository
{
    public ValueTask WriteAsync(InverterStatus inverterStatus, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask<InverterTelemetryResult> QueryAsync(TimeSpan timeSpan, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new InverterTelemetryResult(new Dictionary<DateTime, InverterStatus>()));
    }
}