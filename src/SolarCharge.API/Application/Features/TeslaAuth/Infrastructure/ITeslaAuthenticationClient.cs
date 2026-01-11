using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;

public interface ITeslaAuthenticationClient
{
    ValueTask<TeslaAuthenticationResult?> GetTokensAsync(string jsonRequest, CancellationToken cancellationToken = default);
}