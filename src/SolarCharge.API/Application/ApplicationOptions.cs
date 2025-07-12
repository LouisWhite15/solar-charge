namespace SolarCharge.API.Application;

public class ApplicationOptions
{
    public const string Application = "Application";

    public string InverterStatusCheckCron { get; set; } = "*/2 * * * *"; // Every 2 minutes by default
    public string EvaluateSolarGenerationCron { get; set; } = "*/10 * * * *"; // Every 10 minutes by default
}