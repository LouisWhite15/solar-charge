using SolarCharge.API.Infrastructure.Inverter;

namespace SolarCharge.API.Application.Features.Inverter;

public class InverterOptions
{
    public const string SectionName = "Inverter";

    public string Url { get; set; } = string.Empty;
    public InverterType Type { get; set; } = InverterType.Unknown;
}