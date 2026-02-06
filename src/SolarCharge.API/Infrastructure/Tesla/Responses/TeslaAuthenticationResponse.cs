using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Responses;

public class TeslaAuthenticationResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;
}