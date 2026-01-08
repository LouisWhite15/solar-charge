namespace SolarCharge.API.Application;

public class FeatureOptions
{
    public const string Features = "Features";
    
    public bool IsInfluxDbEnabled { get; set; } = true;
    public bool IsChatBotEnabled { get; set; } = true;
}