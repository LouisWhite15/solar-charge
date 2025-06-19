using System.Text.Json;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Domain.ValueObjects;
using SolarCharge.API.Infrastructure.Inverter.Dtos;

namespace SolarCharge.API.Infrastructure.Inverter;

public class InverterService : IInverter
{
    public const string HttpClientName = "Inverter";
    
    private readonly ILogger<InverterService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string _inverterUrl;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public InverterService(
        ILogger<InverterService> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<InverterOptions> infrastructureOptions)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;

        _inverterUrl = infrastructureOptions.Value.InverterUrl;
    }
    
    public async Task<InverterStatus> GetAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientName);
        httpClient.BaseAddress = new Uri(_inverterUrl);

        var response = await httpClient.GetAsync("/api/status/powerflow");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var inverterStatusDto = JsonSerializer.Deserialize<InverterStatusDto>(content, _jsonSerializerOptions);

        if (inverterStatusDto is null)
        {
            _logger.LogError("Could not deserialize inverter status response");
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("JSON content: {JsonContent}", content);
            }
            
            return new InverterStatus(0, 0, 0);
        }

        return new InverterStatus(
            inverterStatusDto.Site.Photovoltaic,
            inverterStatusDto.Site.Grid,
            inverterStatusDto.Site.Load);
    }
}