using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Dtos;

public class TeslaAuthenticationResult
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;
}