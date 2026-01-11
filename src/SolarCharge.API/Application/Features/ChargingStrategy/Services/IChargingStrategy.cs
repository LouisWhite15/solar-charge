using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public interface IChargingStrategy
{
    ValueTask EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default);
}