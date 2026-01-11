using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SolarCharge.API.Application.Features.TeslaAuth.Services;

public interface ITeslaAuthenticationService
{
    /// <summary>
    /// Retrieve the authentication parameters that are required for authentication via the Tesla auth API
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetAuthenticationParameters();
}

public class TeslaAuthenticationService : ITeslaAuthenticationService
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

    private static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var charArray = Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray();
        return new string(charArray);
    }
}