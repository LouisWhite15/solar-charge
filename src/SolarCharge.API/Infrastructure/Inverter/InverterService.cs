using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Infrastructure.Inverter;

public class InverterService(
    IServiceProvider serviceProvider,
    IOptions<InverterOptions> options) 
    : IInverter
{
    private readonly IInverter _inverter = serviceProvider.GetRequiredKeyedService<IInverter>(options.Value.Type);
    
    public Task<InverterStatus> GetAsync()
    {
        // Pass the request on to the inverter that is configured to be used
        return _inverter.GetAsync();
    }
}