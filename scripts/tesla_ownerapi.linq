<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

async Task Main()
{
	// Provide Tesla access token to authenticate with the Tesla Owner API
	var accessToken = "";
	
	var httpClient = new HttpClient
	{
		BaseAddress = new Uri("https://owner-api.teslamotors.com")
	};
	httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
	
	var jsonSerializerOptions = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true,
		NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString
	};
	jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	
	var productResponse = await httpClient.GetAsync("/api/1/products");
	var productsContent = await productResponse.Content.ReadAsStringAsync();
	productsContent.Dump("products");

	var products = JsonSerializer.Deserialize<ProductsResponse>(productsContent, jsonSerializerOptions);
	products.Dump("Products");

	var teslaVehicleId = products.Products.Single().Id;
	
	var stopwatch = Stopwatch.StartNew();
	var vehicleResponse = await httpClient.GetAsync($"/api/1/vehicles/{teslaVehicleId}");
	
	var vehicleContent = await vehicleResponse.Content.ReadAsStringAsync();
	vehicleContent.Dump("vehicle");

	var vehicleData = JsonSerializer.Deserialize<VehicleResponse>(vehicleContent, jsonSerializerOptions);
	vehicleData.Dump("Vehicle");
}

public class ProductsResponse
{
	[JsonPropertyName("response")]
	public Product[] Products { get; set; }
}

public class Product
{
	public long Id { get; set; }
}

public class VehicleResponse
{
	public VehicleData Response { get; set; }
}

public class VehicleData
{
	public long Id { get; set; }

	[JsonPropertyName("display_name")]
	public string DisplayName { get; set; }

	public VehicleStateDto State { get; set; }
}

public enum VehicleStateDto
{
	Unknown = 0,
	Offline = 1,
	Asleep = 2,
	Online = 3
}
