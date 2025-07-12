namespace SolarCharge.API.Application.Features;

public class FeatureOptions
{
    public const string Features = "Features";
    
    public bool IsInfluxDbEnabled { get; set; } = true;
}