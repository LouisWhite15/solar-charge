using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class ChatBotService(
    ILogger<ChatBotService> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<ChatBotOptions> options)
    : IChatBot
{
    private readonly string _chatBotBaseUrl = options.Value.Url;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString
    };

    public async Task SendMessageAsync(string messageText)
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
            new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        
        logger.LogDebug("Message sent successfully");
    }
}