using System.Text.Json;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.Inverter;
using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Infrastructure;
using SolarCharge.API.Infrastructure.Inverter.Fronius.Dtos;

namespace SolarCharge.API.Infrastructure.Inverter.Fronius;

public class FroniusInverterClientService(
    ILogger<FroniusInverterClientService> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<InverterOptions> inverterOptions)
    : IInverterClient
{
    private const string HttpClientName = "FroniusInverter";

    private readonly string _inverterUrl = inverterOptions.Value.Url;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<InverterStatus> GetAsync(CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Retrieving Fronius inverter status");
        
        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        httpClient.BaseAddress = new Uri(_inverterUrl);

        var response = await httpClient.GetAsync("/api/status/powerflow", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var inverterStatusDto = JsonSerializer.Deserialize<InverterStatusDto>(content, _jsonSerializerOptions);

        if (inverterStatusDto is null)
        {
            logger.LogError("Could not deserialize inverter status response");
            return new InverterStatus(0, 0, 0);
        }

        return new InverterStatus(
            inverterStatusDto.Site.Photovoltaic,
            inverterStatusDto.Site.Grid,
            inverterStatusDto.Site.Load);
    }
}