using SolarCharge.API.Application.Features.TeslaAuth.Domain;

namespace SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;

public interface ITeslaAuthenticationRepository
{
    ValueTask<TeslaAuthentication?> GetAsync(CancellationToken cancellationToken = default);
    ValueTask SetAsync(TeslaAuthentication teslaAuthentication, CancellationToken cancellationToken = default);
}