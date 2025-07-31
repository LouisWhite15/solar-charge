using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Ports;

public interface ITeslaAuthenticationRepository
{
    Task<TeslaAuthentication?> GetAsync();
    Task SetAsync(TeslaAuthentication teslaAuthentication);
}