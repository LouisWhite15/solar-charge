using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Infrastructure;

namespace SolarCharge.API.Infrastructure.Inverter;

public class NoOpInverterClientService(ILogger<NoOpInverterClientService> logger) : IInverterClient
{
    public Task<InverterStatus> GetAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("NoOpInverterService is currently registered. Returning default value");
        return Task.FromResult(new InverterStatus(0, 0, 0));
    }
}