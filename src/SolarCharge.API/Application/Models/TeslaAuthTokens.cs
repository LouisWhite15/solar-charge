using System.Text.Json.Serialization;

namespace SolarCharge.API.Application.Models;

public class TeslaAuthTokens
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
	
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}