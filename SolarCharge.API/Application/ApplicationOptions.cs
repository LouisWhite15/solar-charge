namespace SolarCharge.API.Application;

public class ApplicationOptions
{
    public const string Application = "Application";

    public int InverterStatusCheckFrequencySeconds { get; set; } = 60;
}