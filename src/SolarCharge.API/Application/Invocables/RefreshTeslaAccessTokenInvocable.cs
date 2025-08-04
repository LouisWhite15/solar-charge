using System.IdentityModel.Tokens.Jwt;
using Coravel.Invocable;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Application.Invocables;

public class RefreshTeslaAccessTokenInvocable(
    ILogger<RefreshTeslaAccessTokenInvocable> logger,
    ITeslaAuthenticationRepository teslaAuthenticationRepository,
    IDateTimeOffsetService dateTimeOffsetService,
    ITeslaAuthenticationService teslaAuthenticationService) : IInvocable
{
    public async Task Invoke()
    {
        logger.LogTrace("Refresh token job started");

        var tokens = await teslaAuthenticationRepository.GetAsync();
        if (tokens is null)
        {
            logger.LogDebug("Not authenticated with the Tesla API");
            return;
        }
        
        var accessTokenExpiry = GetJwtExpiration(tokens.AccessToken);
        var now = dateTimeOffsetService.UtcNow;

        if (accessTokenExpiry.AddHours(-2) >= now)
        {
            logger.LogDebug("Token does not need to be refreshed yet");
            return;
        }
        
        logger.LogDebug("Token will be refreshed");
        await teslaAuthenticationService.RefreshAsync();
        
        logger.LogTrace("Refresh token job completed");
    }

    private static DateTime GetJwtExpiration(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        return token.ValidTo;
    }
}