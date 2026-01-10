using SolarCharge.API.Application.Features.Inverter.Domain;

namespace SolarCharge.API.Application.Features.Inverter.Infrastructure;

public interface IInverterClient
{
    Task<InverterStatus> GetAsync(CancellationToken cancellationToken = default);
}