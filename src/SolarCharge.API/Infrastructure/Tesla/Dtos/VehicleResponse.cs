using System.Text.Json.Serialization;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Infrastructure.Tesla.Dtos;

public class VehicleResponse
{
    public VehicleData Response { get; set; }
}

public class VehicleData
{
    public long Id { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
    
    public VehicleStateDto State { get; set; }
}
