namespace SolarCharge.API.Application.Features.ChargingStrategy;

public class ChargingStrategyOptions
{
    public const string SectionName = "ChargingStrategy";
    
    public int StartChargingExcessGenerationThresholdWatts { get; set; } = 2000;
    public int StopChargingPullingFromGridThresholdWatts { get; set; } = 500;
}