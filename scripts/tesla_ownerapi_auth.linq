<Query Kind="Program">
  <NuGetReference>Microsoft.IdentityModel.Tokens</NuGetReference>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>Microsoft.IdentityModel.Tokens</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Web</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

HttpClient _httpClient;

async Task Main()
{
	_httpClient = new HttpClient();
	_httpClient.DefaultRequestHeaders.Add("User-Agent", "solar-charge");
	
	await AuthenticateAsync();
}

public async Task AuthenticateAsync()
{
	var codeVerifier = GetRandomString(86);
	var codeChallenge = Base64UrlEncoder.Encode(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(codeVerifier)));
	
	var state = GetRandomString(20);

	var queryParameters = new Dictionary<string, string>
	{
		{"client_id", "ownerapi"},
		{"code_challenge", codeChallenge},
		{"code_challenge_method", "S256"},
		{"redirect_uri", "https://auth.tesla.com/void/callback"},
		{"response_type", "code"},
		{"scope", "openid email offline_access"},
		{"state", state}
	};

	var queryString = BuildQueryString(queryParameters);

	// Step 1: Login to Tesla using the following link
	var teslaLogonLink = $"https://auth.tesla.com/oauth2/v3/authorize?{queryString}";
	new Hyperlinq(teslaLogonLink).Dump("Tesla Logon Link");
	
	// Step 2: Provide the URL with the code
	var finalUrl = await Util.ReadLineAsync("Please enter the URL from the browser at the 'Page not Found' page");
	var finalUri = new Uri(finalUrl);
	var queryParams = HttpUtility.ParseQueryString(finalUri.Query);
	var code = queryParams["code"];
	code.Dump("Code");

	// Step 3: Exchange the authorization code for a bearer token
	var requestParameters = new Dictionary<string, string>()
	{
		{ "grant_type", "authorization_code" },
		{ "client_id", "ownerapi" },
		{ "code", code },
		{ "code_verifier", codeVerifier },
		{ "redirect_uri", "https://auth.tesla.com/void/callback" }
	};
	var jsonRequest = JsonSerializer.Serialize(requestParameters);
	var tokenResponse = await _httpClient.PostAsync("https://auth.tesla.com/oauth2/v3/token", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
	var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
	
	if (!tokenResponse.IsSuccessStatusCode)
	{
		$"Request to retrieve tokens failed. Status Code: {tokenResponse.StatusCode}".Dump();
		tokenContent.Dump("Content");
	}
	
	var tokens = JsonSerializer.Deserialize<TeslaAuthTokens>(tokenContent);
	tokens.Dump("Tesla Auth Tokens");
}

private string GetRandomString(int length)
{
	var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	var random = new Random();
	var charArray = Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray();
	return new string(charArray);
}

private static string BuildQueryString(Dictionary<string, string> parameters)
{
	var list = new List<string>();
	foreach (var kvp in parameters)
	{
		string encodedKey = Uri.EscapeDataString(kvp.Key);
		string encodedValue = Uri.EscapeDataString(kvp.Value);
		list.Add($"{encodedKey}={encodedValue}");
	}
	return string.Join("&", list);
}

public class TeslaAuthTokens
{
	[JsonPropertyName("access_token")]
	public string AccessToken { get; set; }
	
	[JsonPropertyName("refresh_token")]
	public string RefreshToken { get; set; }
}
