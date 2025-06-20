using SolarCharge.API.Application.Services;
using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Infrastructure.Inverter;

public class NoOpInverterService : IInverter
{
    public NoOpInverterService(ILogger<NoOpInverterService> logger)
    {
        logger.LogWarning("NoOpInverterService is currently being used");
    }
    
    public Task<InverterStatus> GetAsync()
    {
        return Task.FromResult(new InverterStatus(0, 0, 0));
    }
}