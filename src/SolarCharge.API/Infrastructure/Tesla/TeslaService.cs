using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaService : ITesla
{
    private readonly ILogger<TeslaService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITeslaAuthenticationRepository _teslaAuthenticationRepository;
    private readonly IOptions<TeslaOptions> _teslaOptions;

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public TeslaService(
        ILogger<TeslaService> logger,
        IHttpClientFactory httpClientFactory,
        ITeslaAuthenticationRepository teslaAuthenticationRepository,
        IOptions<TeslaOptions> teslaOptions)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _teslaAuthenticationRepository = teslaAuthenticationRepository;
        _teslaOptions = teslaOptions;
        
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString
        };
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
    
    public async Task<VehicleDto?> GetVehicleAsync()
    {
        _logger.LogTrace("Retrieving vehicle id from Tesla");

        var teslaAuthTokens = await _teslaAuthenticationRepository.GetAsync();
        if (teslaAuthTokens is null)
        {
            _logger.LogError("Not authenticated with Tesla");
            return null;
        }
        
        var httpClient = _httpClientFactory.CreateClient("tesla-owner-api");
        httpClient.BaseAddress = new Uri(_teslaOptions.Value.TeslaApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teslaAuthTokens.AccessToken);

        var productsHttpResponse = await httpClient.GetAsync("/api/1/products");
        if (!productsHttpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Could not retrieve products from Tesla. StatusCode: {StatusCode}", productsHttpResponse.StatusCode);
            return null;
        }
        
        var productsContent = await productsHttpResponse.Content.ReadAsStringAsync();
        var productsResponse = JsonSerializer.Deserialize<ProductsResponse>(productsContent, _jsonSerializerOptions);

        var product = productsResponse?.Products.FirstOrDefault();
        return product is null
            ? null
            : new VehicleDto(product.Id, product.DisplayName, VehicleStateDto.Unknown);
    }

    public async Task<VehicleDto?> GetVehicleStateAsync(VehicleDto vehicle)
    {
        _logger.LogTrace("Retrieving charge state from Tesla");

        var teslaAuthTokens = await _teslaAuthenticationRepository.GetAsync();
        if (teslaAuthTokens is null)
        {
            _logger.LogError("Not authenticated with Tesla");
            return null;
        }
        
        var httpClient = _httpClientFactory.CreateClient("tesla-owner-api");
        httpClient.BaseAddress = new Uri(_teslaOptions.Value.TeslaApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teslaAuthTokens.AccessToken);

        var vehicleHttpResponse = await httpClient.GetAsync($"/api/1/vehicles/{vehicle.Id}");
        if (!vehicleHttpResponse.IsSuccessStatusCode)
        {
            _logger.LogWarning("Could not retrieve vehicle data from Tesla. StatusCode: {StatusCode}", vehicleHttpResponse.StatusCode);
            return null;
        }
        
        var vehicleContent = await vehicleHttpResponse.Content.ReadAsStringAsync();
        var vehicleResponse = JsonSerializer.Deserialize<VehicleResponse>(vehicleContent, _jsonSerializerOptions);
        if (vehicleResponse?.Response is null)
        {
            _logger.LogWarning("Could not parse vehicle API response");
            return null;
        }
        
        _logger.LogDebug("State retrieved from Tesla. Id: {Id}. State: {State}", 
            vehicleResponse.Response.Id, vehicleResponse.Response.State);

        return new VehicleDto(vehicleResponse);
    }
}