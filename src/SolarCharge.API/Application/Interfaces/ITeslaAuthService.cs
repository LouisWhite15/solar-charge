using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Interfaces;

public interface ITeslaAuthService
{
    /// <summary>
    /// Retrieve the authentication parameters that are required for authentication via the Tesla auth API
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetAuthenticationParameters();
    
    /// <summary>
    /// Authenticate to the Tesla Owner API using the URL from the users login as well as the authentication parameters that were used to generate the URL
    /// </summary>
    /// <param name="urlWithCode"></param>
    /// <param name="authenticationParameters"></param>
    /// <returns></returns>
    Task<TeslaAuthTokens> AuthenticateAsync(string urlWithCode, Dictionary<string, string> authenticationParameters);
    
    /// <summary>
    /// Authenticate to the Tesla Owner API using provided auth token and refresh token
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="refreshToken"></param>
    Task AuthenticateAsync(string authToken, string refreshToken);

    /// <summary>
    /// Refresh the auth token
    /// </summary>
    Task RefreshAsync();
}