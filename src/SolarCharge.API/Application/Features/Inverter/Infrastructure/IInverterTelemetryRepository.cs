using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Application.Features.Inverter.Infrastructure;

public interface IInverterTelemetryRepository
{
    ValueTask WriteAsync(InverterStatus inverterStatus, CancellationToken cancellationToken = default);
    ValueTask<InverterTelemetryResult> QueryAsync(TimeSpan timeSpan, CancellationToken cancellationToken = default);
}