using SolarCharge.API.Application.Features.ChargingStrategy.Commands;
using SolarCharge.API.Application.Features.Inverter.Queries;

namespace SolarCharge.API.Application.Features.ChargingStrategy.Services;

public interface IChargingStrategy
{
    bool CanEvaluate(ExecuteChargingStrategyCommand command);
    Task EvaluateAsync(InverterTelemetryResult inverterTelemetryResult, CancellationToken cancellationToken = default);
}