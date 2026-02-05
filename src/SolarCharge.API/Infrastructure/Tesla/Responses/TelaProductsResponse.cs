using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Responses;

public class TelaProductsResponse
{
    [JsonPropertyName("response")]
    public required TeslaProduct[] Products { get; init; }
}

public class TeslaProduct
{
    public required long Id { get; set; }
    
    [JsonPropertyName("display_name")]
    public required string DisplayName { get; set; }
}
