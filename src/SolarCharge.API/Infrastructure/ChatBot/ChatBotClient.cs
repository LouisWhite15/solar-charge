using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.ChatBot.Infrastructure;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class ChatBotClient(
    ILogger<ChatBotClient> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<ChatBotOptions> options)
    : IChatBotClient
{
    private readonly string _chatBotBaseUrl = options.Value.Url;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString
    };

    public async Task SendMessageAsync(string messageText, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Sending message to user...");

        var httpClient = httpClientFactory.CreateClient("ChatBotClient");

        var request = new
        {
            MessageText = messageText
        };
        var jsonRequest = JsonSerializer.Serialize(request, JsonSerializerOptions);
        var response = await httpClient.PostAsync(
            $"{_chatBotBaseUrl}/telegram/send",
            new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
            cancellationToken);

        response.EnsureSuccessStatusCode();
        
        logger.LogDebug("Message sent successfully");
    }
}