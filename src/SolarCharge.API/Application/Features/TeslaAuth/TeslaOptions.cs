namespace SolarCharge.API.Application.Features.TeslaAuth;

public class TeslaOptions
{
    public const string SectionName = "Tesla";

    public string EncryptionKey { get; set; } = string.Empty;
    public string TeslaAuthenticationUrl { get; set; } = "https://auth.tesla.com/oauth2/v3/token";
}