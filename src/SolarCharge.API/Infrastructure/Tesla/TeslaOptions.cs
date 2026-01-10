namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaOptions
{
    public const string SectionName = "Tesla";

    public string EncryptionKey { get; set; } = string.Empty;
    public string TeslaAuthenticationUrl { get; set; } = "https://auth.tesla.com/oauth2/v3/token";
    public string TeslaApiUrl { get; set; } = "https://owner-api.teslamotors.com";
}