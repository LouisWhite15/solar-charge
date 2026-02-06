using SolarCharge.API.Infrastructure.Tesla.Responses;

namespace SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;

public interface ITeslaAuthenticationClient
{
    ValueTask<TeslaAuthenticationResponse?> GetTokensAsync(string jsonRequest, CancellationToken cancellationToken = default);
}