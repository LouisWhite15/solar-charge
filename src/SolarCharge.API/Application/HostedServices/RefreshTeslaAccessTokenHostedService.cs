using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Application.HostedServices;

public class RefreshTeslaAccessTokenHostedService(
    ILogger<RefreshTeslaAccessTokenHostedService> logger,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeOffsetService dateTimeOffsetService,
    IOptions<ApplicationOptions> applicationOptions) 
    : AsyncTimedHostedService(logger, applicationOptions.Value.RefreshTeslaAccessTokenFrequencySeconds)
{
    protected override async Task DoWorkAsync()
    {
        logger.LogTrace("Refresh token job started");
        
        using var scope = serviceScopeFactory.CreateScope();
        var teslaAuthenticationRepository = scope.ServiceProvider.GetRequiredService<ITeslaAuthenticationRepository>();

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

        var teslaAuthentication = scope.ServiceProvider.GetRequiredService<ITeslaAuthentication>();
        await teslaAuthentication.RefreshAsync();
        
        logger.LogTrace("Refresh token job completed");
    }
    
    private static DateTime GetJwtExpiration(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        return token.ValidTo;
    }
}