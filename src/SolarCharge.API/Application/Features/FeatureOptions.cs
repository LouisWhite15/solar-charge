namespace SolarCharge.API.Application.Features;

public class FeatureOptions
{
    public const string SectionName = "Features";
    
    public bool IsInfluxDbEnabled { get; set; } = true;
    public bool IsChatBotEnabled { get; set; } = true;
}