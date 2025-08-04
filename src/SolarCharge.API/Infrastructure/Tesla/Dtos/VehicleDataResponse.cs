using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Dtos;

public class VehicleDataResponse
{
    [JsonPropertyName("response")]
    public VehicleData Vehicle { get; set; }
}

public class VehicleData
{
    public long Id { get; set; }
	
    [JsonPropertyName("charge_state")]
    public ChargeStateDto ChargeState { get; set; }
}

public class ChargeStateDto
{
    [JsonPropertyName("charging_state")]
    public string ChargingState { get; set; }
}