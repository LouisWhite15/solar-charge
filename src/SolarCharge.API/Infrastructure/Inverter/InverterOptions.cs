namespace SolarCharge.API.Infrastructure.Inverter;

public class InverterOptions
{
    public const string Inverter = "Inverter";

    public string Url { get; set; } = string.Empty;
    public InverterType Type { get; set; } = InverterType.Unknown;
}