namespace SolarCharge.API.Infrastructure.InfluxDB;

public class InfluxDbOptions
{
    public const string SectionName = "InfluxDB";

    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}