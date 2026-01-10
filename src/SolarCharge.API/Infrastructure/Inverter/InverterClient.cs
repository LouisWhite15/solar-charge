using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.Inverter;
using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Infrastructure;

namespace SolarCharge.API.Infrastructure.Inverter;

public class InverterClient(
    IServiceProvider serviceProvider,
    IOptions<InverterOptions> options) 
    : IInverterClient
{
    private readonly IInverterClient _inverterClient = serviceProvider.GetRequiredKeyedService<IInverterClient>(options.Value.Type);
    
    public Task<InverterStatus> GetAsync(CancellationToken cancellationToken = default)
    {
        // Pass the request on to the inverter that is configured to be used
        return _inverterClient.GetAsync(cancellationToken);
    }
}