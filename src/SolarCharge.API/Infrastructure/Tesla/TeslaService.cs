using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Domain.ValueObjects;
using SolarCharge.API.Infrastructure.Tesla.Dtos;
using SolarCharge.API.Infrastructure.Tesla.Extensions;
using ChargeStateDto = SolarCharge.API.Application.Models.ChargeStateDto;

namespace SolarCharge.API.Infrastructure.Tesla;

public class TeslaService(
    ILogger<TeslaService> logger,
    IHttpClientFactory httpClientFactory,
    ITeslaAuthenticationRepository teslaAuthenticationRepository)
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
        httpClient.BaseAddress = new Uri("https://owner-api.teslamotors.com");
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
            : new VehicleDto(product.Id, product.DisplayName, ChargeStateDto.Unknown);
    }

    public async Task<ChargeStateDto> GetChargeStateAsync(long vehicleId)
    {
        logger.LogTrace("Retrieving charge state from Tesla");

        var teslaAuthTokens = await teslaAuthenticationRepository.GetAsync();
        if (teslaAuthTokens is null)
        {
            logger.LogError("Not authenticated with Tesla");
            return ChargeStateDto.Unknown;
        }
        
        var httpClient = httpClientFactory.CreateClient("tesla-owner-api");
        httpClient.BaseAddress = new Uri("https://owner-api.teslamotors.com");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teslaAuthTokens.AccessToken);

        var vehicleDataHttpResponse = await httpClient.GetAsync($"/api/1/vehicles/{vehicleId}/vehicle_data");
        if (!vehicleDataHttpResponse.IsSuccessStatusCode)
        {
            logger.LogWarning("Could not retrieve vehicle data from Tesla. StatusCode: {StatusCode}. This could be because the Tesla is asleep or offline", vehicleDataHttpResponse.StatusCode);
            return ChargeStateDto.Unknown;
        }
        
        var vehicleDataContent = await vehicleDataHttpResponse.Content.ReadAsStringAsync();
        var vehicleDataResponse = JsonSerializer.Deserialize<VehicleDataResponse>(vehicleDataContent, JsonSerializerOptions);

        return vehicleDataResponse?.Vehicle.ChargeState.ChargingState.ToDto() ?? ChargeStateDto.Unknown;
    }

    public Task StartChargingAsync()
    {
        throw new NotImplementedException();
    }

    public Task StopChargingAsync()
    {
        throw new NotImplementedException();
    }
}