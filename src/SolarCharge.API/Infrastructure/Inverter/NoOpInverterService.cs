using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Infrastructure.Inverter;

public class NoOpInverterService(ILogger<NoOpInverterService> logger) : IInverter
{
    public Task<InverterStatus> GetAsync()
    {
        logger.LogDebug("NoOpInverterService is currently registered. Returning default value");
        return Task.FromResult(new InverterStatus(0, 0, 0));
    }
}