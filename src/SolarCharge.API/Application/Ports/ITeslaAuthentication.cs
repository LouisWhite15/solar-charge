namespace SolarCharge.API.Application.Ports;

public interface ITeslaAuthentication
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
    Task<bool> AuthenticateAsync(string urlWithCode, Dictionary<string, string> authenticationParameters);

    /// <summary>
    /// Refresh the access token
    /// </summary>
    Task RefreshAsync();
}