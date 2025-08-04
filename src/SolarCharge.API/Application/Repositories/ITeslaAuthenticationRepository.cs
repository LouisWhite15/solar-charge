using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Repositories;

public interface ITeslaAuthenticationRepository
{
    Task<TeslaAuthentication?> GetAsync();
    Task SetAsync(TeslaAuthentication teslaAuthentication);
}