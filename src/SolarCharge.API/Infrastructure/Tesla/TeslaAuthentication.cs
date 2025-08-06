using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaAuthentication(
    ILogger<TeslaAuthentication> logger,
    IHttpClientFactory httpClientFactory,
    ITeslaAuthenticationRepository teslaAuthenticationRepository)
    : ITeslaAuthentication
{
    public Dictionary<string, string> GetAuthenticationParameters()
    {
        var codeVerifier = GetRandomString(86);
        var codeChallenge = Base64UrlEncoder.Encode(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)));
        var state = GetRandomString(20);

        return new Dictionary<string, string>
        {
            {"client_id", "ownerapi"},
            {"code_verifier", codeVerifier},
            {"code_challenge", codeChallenge},
            {"code_challenge_method", "S256"},
            {"redirect_uri", "https://auth.tesla.com/void/callback"},
            {"response_type", "code"},
            {"scope", "openid email offline_access"},
            {"state", state}
        };
    }

    public async Task<bool> AuthenticateAsync(string urlWithCode, Dictionary<string, string> authenticationParameters)
    {
        var finalUri = new Uri(urlWithCode);
        var queryParams = HttpUtility.ParseQueryString(finalUri.Query);
        var code = queryParams["code"];
        
        var requestParameters = new Dictionary<string, string>()
        {
            { "grant_type", "authorization_code" },
            { "client_id", authenticationParameters["client_id"] },
            { "code", code ?? string.Empty },
            { "code_verifier", authenticationParameters["code_verifier"] },
            { "redirect_uri", authenticationParameters["redirect_uri"] }
        };
        var jsonRequest = JsonSerializer.Serialize(requestParameters);

        var tokens = await GetTeslaAuthenticationTokens(jsonRequest);
        if (tokens is null)
        {
            logger.LogError("Tesla Authentication Tokens could not be retrieved");
            return false;
        }
        
        logger.LogDebug("Persisting Tesla Authentication Tokens");
        await teslaAuthenticationRepository.SetAsync(new Application.Models.TeslaAuthentication(tokens.AccessToken, tokens.RefreshToken));

        return true;
    }
    
    public async Task RefreshAsync()
    {
        var existingTokens = await teslaAuthenticationRepository.GetAsync();
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
        
        var tokens = await GetTeslaAuthenticationTokens(jsonRequest);
        if (tokens is null)
        {
            logger.LogError("Tesla Authentication Tokens could not be retrieved");
            return;
        }
        
        logger.LogDebug("Persisting Tesla Authentication Tokens");
        await teslaAuthenticationRepository.SetAsync(new Application.Models.TeslaAuthentication(tokens.AccessToken, tokens.RefreshToken));
    }
    
    private async Task<TeslaAuthenticationResult?> GetTeslaAuthenticationTokens(string jsonRequest)
    {
        var httpClient = httpClientFactory.CreateClient("tesla-auth-client");
        var tokenResponse = await httpClient.PostAsync("https://auth.tesla.com/oauth2/v3/token", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        
        if (!tokenResponse.IsSuccessStatusCode)
        {
            logger.LogError("Error authenticating Tesla user. StatusCode: {StatusCode}. Content: {Content}", tokenResponse.StatusCode, tokenContent);
            return null;
        }
        
        var tokens = JsonSerializer.Deserialize<TeslaAuthenticationResult>(tokenContent);
        return tokens;
    }

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var charArray = Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray();
        return new string(charArray);
    }
}