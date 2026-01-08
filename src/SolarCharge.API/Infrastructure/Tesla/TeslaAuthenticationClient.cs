using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaAuthenticationClient(
    ILogger<TeslaAuthenticationClient> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<TeslaOptions> teslaOptions) 
    : ITeslaAuthenticationClient
{
    public async ValueTask<TeslaAuthenticationResult?> GetTokensAsync(string jsonRequest, CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateClient("tesla-auth-client");
        var tokenResponse = await httpClient.PostAsync(
            teslaOptions.Value.TeslaAuthenticationUrl,
            new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
            cancellationToken);
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
        
        if (!tokenResponse.IsSuccessStatusCode)
        {
            logger.LogError("Error authenticating Tesla user. StatusCode: {StatusCode}. Content: {Content}", tokenResponse.StatusCode, tokenContent);
            return null;
        }
        
        var tokens = JsonSerializer.Deserialize<TeslaAuthenticationResult>(tokenContent);
        return tokens;
    }
}