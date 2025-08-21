namespace SolarCharge.API.Application;

public class ApplicationOptions
{
    public const string Application = "Application";

    public int InverterStatusCheckFrequencySeconds { get; set; } = 120; // Every 2 minutes by default
    public int EvaluateSolarGenerationFrequencySeconds { get; set; } = 600; // Every 10 minutes by default
    public int RefreshTeslaAccessTokenFrequencySeconds { get; set; } = 7200; // Every 2 hours by default
    public int ExcessGenerationToChargeThresholdWatts { get; set; } = 2000;
}