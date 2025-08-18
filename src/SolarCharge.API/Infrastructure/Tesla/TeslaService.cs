using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaService(
    ILogger<TeslaService> logger,
    IHttpClientFactory httpClientFactory,
    ITeslaAuthenticationRepository teslaAuthenticationRepository,
    IOptions<TeslaOptions> teslaOptions)
    : ITesla
{

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString
    };
    
    public async Task<VehicleDto?> GetVehicleAsync()
    {
        logger.LogTrace("Retrieving vehicle id from Tesla");

        var teslaAuthTokens = await teslaAuthenticationRepository.GetAsync();
        if (teslaAuthTokens is null)
        {
            logger.LogError("Not authenticated with Tesla");
            return null;
        }
        
        var httpClient = httpClientFactory.CreateClient("tesla-owner-api");
        httpClient.BaseAddress = new Uri(teslaOptions.Value.TeslaApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teslaAuthTokens.AccessToken);

        var productsHttpResponse = await httpClient.GetAsync("/api/1/products");
        if (!productsHttpResponse.IsSuccessStatusCode)
        {
            logger.LogError("Could not retrieve products from Tesla. StatusCode: {StatusCode}", productsHttpResponse.StatusCode);
            return null;
        }
        
        var productsContent = await productsHttpResponse.Content.ReadAsStringAsync();
        var productsResponse = JsonSerializer.Deserialize<ProductsResponse>(productsContent, JsonSerializerOptions);

        var product = productsResponse?.Products.FirstOrDefault();
        return product is null
            ? null
            : new VehicleDto(product.Id, product.DisplayName, VehicleStateDto.Unknown);
    }

    public async Task<VehicleDto?> GetVehicleStateAsync(VehicleDto vehicle)
    {
        logger.LogTrace("Retrieving charge state from Tesla");

        var teslaAuthTokens = await teslaAuthenticationRepository.GetAsync();
        if (teslaAuthTokens is null)
        {
            logger.LogError("Not authenticated with Tesla");
            return null;
        }
        
        var httpClient = httpClientFactory.CreateClient("tesla-owner-api");
        httpClient.BaseAddress = new Uri(teslaOptions.Value.TeslaApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teslaAuthTokens.AccessToken);

        var vehicleHttpResponse = await httpClient.GetAsync($"/api/1/vehicles/{vehicle.Id}");
        if (!vehicleHttpResponse.IsSuccessStatusCode)
        {
            logger.LogWarning("Could not retrieve vehicle data from Tesla. StatusCode: {StatusCode}", vehicleHttpResponse.StatusCode);
            return null;
        }
        
        var vehicleContent = await vehicleHttpResponse.Content.ReadAsStringAsync();
        var vehicleResponse = JsonSerializer.Deserialize<VehicleResponse>(vehicleContent, JsonSerializerOptions);
        if (vehicleResponse?.Response is null)
        {
            logger.LogWarning("Could not parse vehicle API response");
            return null;
        }
        
        logger.LogDebug("State retrieved from Tesla. Id: {Id}. State: {State}", 
            vehicleResponse.Response.Id, vehicleResponse.Response.State);

        return new VehicleDto(vehicleResponse);
    }
}