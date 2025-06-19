using System.Text.Json.Serialization;

namespace SolarCharge.API.Infrastructure.Inverter.Dtos;

public class SiteDto
{
    [JsonPropertyName("P_PV")]
    public double Photovoltaic { get; set; }
    
    [JsonPropertyName("P_Grid")]
    public double Grid { get; set; }
    
    [JsonPropertyName("P_Load")]
    public double Load { get; set; }
}