using System.Text.Json;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Interfaces;
using SolarCharge.API.Domain.ValueObjects;
using SolarCharge.API.Infrastructure.Inverter.Dtos;

namespace SolarCharge.API.Infrastructure.Inverter.Fronius;

public class FroniusInverterService(
    ILogger<FroniusInverterService> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<InverterOptions> infrastructureOptions)
    : IInverter
{
    private const string HttpClientName = "FroniusInverter";

    private readonly string _inverterUrl = infrastructureOptions.Value.Url;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<InverterStatus> GetAsync()
    {
        logger.LogTrace("Retrieving Fronius inverter status");
        
        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        httpClient.BaseAddress = new Uri(_inverterUrl);

        var response = await httpClient.GetAsync("/api/status/powerflow");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
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