using System.Text.Json;
using SolarCharge.API.Application.Features.TeslaAuth.Domain;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using Wolverine;

namespace SolarCharge.API.Application.Features.TeslaAuth.Refresh;

public sealed class RefreshTeslaCommand
{
    public class Handler(
        ILogger<Handler> logger,
        ITeslaAuthenticationRepository teslaAuthenticationRepository,
        ITeslaAuthenticationClient teslaAuthenticationClient)
        : IWolverineHandler
    {
        public async ValueTask HandleAsync(RefreshTeslaCommand _, CancellationToken cancellationToken)
        {
            var existingTokens = await teslaAuthenticationRepository.GetAsync(cancellationToken);
            if (existingTokens is null)
            {
                logger.LogError("Tesla Authentication Tokens could not be retrieved");
                return;
            }
        
            var requestParameters = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "client_id", "ownerapi" },
                { "refresh_token", existingTokens.RefreshToken },
                { "scope", "openid email offline_access" },
            };
            var jsonRequest = JsonSerializer.Serialize(requestParameters);
        
            var tokens = await teslaAuthenticationClient.GetTokensAsync(jsonRequest, cancellationToken);
            if (tokens is null)
            {
                logger.LogError("Tesla Authentication Tokens could not be retrieved");
                return;
            }
        
            logger.LogDebug("Persisting Tesla Authentication Tokens");
            await teslaAuthenticationRepository.SetAsync(new TeslaAuthentication(tokens.AccessToken, tokens.RefreshToken), cancellationToken);
        }
    }
}