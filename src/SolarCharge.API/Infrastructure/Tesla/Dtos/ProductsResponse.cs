using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Dtos;

public class ProductsResponse
{
    [JsonPropertyName("response")]
    public Product[] Products { get; set; }
}

public class Product
{
    public long Id { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
}
