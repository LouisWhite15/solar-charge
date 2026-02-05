using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Responses;

public class TeslaVehicleResponse
{
    public required TeslaVehicle Response { get; set; }
}

public class TeslaVehicle
{
    public required long Id { get; set; }
    
    [JsonPropertyName("display_name")]
    public required string DisplayName { get; set; }
    
    public required TeslaVehicleState State { get; set; }
}

public enum TeslaVehicleState
{
    Unknown = 0,
    Offline = 1,
    Asleep = 2,
    Online = 3
}