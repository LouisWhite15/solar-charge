namespace SolarCharge.API.Application.Features.Vehicles;

public class VehicleOptions
{
    public const string SectionName = "Vehicle";
    
    public string TeslaApiUrl { get; set; } = "https://owner-api.teslamotors.com";
}