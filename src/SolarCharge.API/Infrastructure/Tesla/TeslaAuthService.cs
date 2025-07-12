using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using SolarCharge.API.Application.Interfaces;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaAuthService : ITeslaAuthService
{
    private readonly ILogger<TeslaAuthService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public TeslaAuthService(
        ILogger<TeslaAuthService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

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

    public async Task<TeslaAuthTokens> AuthenticateAsync(string urlWithCode, Dictionary<string, string> authenticationParameters)
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
        
        var httpClient = _httpClientFactory.CreateClient("tesla-auth-client");
        var tokenResponse = await httpClient.PostAsync("https://auth.tesla.com/oauth2/v3/token", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
        tokenResponse.EnsureSuccessStatusCode();
        
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokens = JsonSerializer.Deserialize<TeslaAuthTokens>(tokenContent);

        return tokens ?? new TeslaAuthTokens();
    }

    public Task AuthenticateAsync(string authToken, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync()
    {
        throw new NotImplementedException();
    }

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var charArray = Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray();
        return new string(charArray);
    }
}