namespace SolarCharge.API.Application;

public class ApplicationOptions
{
    public const string Application = "Application";

    public string InverterStatusCheckCron { get; set; } = "*/5 * * * *";
}