using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Tesla.Responses;

public class TeslaVehicleDataResponse
{
    public TeslaVehicleData? Response { get; set; }
}

public class TeslaVehicleData
{
    public required long Id { get; set; }
	
    [JsonPropertyName("charge_state")]
    public required TeslaChargeState ChargeState { get; set; }
}

public class TeslaChargeState
{
    [JsonPropertyName("charging_state")]
    public TeslaChargingState ChargingState { get; set; }
}

public enum TeslaChargingState
{
    Unknown = 0,
    Disconnected = 1,
    Stopped = 2,
    Charging = 3,
    Complete = 4
}