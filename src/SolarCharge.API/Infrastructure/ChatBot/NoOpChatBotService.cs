using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class NoOpChatBotService(ILogger<NoOpChatBotService> logger) : IChatBot
{
    public Task SendMessageAsync(string messageText)
    {
        logger.LogDebug("ChatBot integration is disabled. Not sending message");
        return Task.CompletedTask;
    }
}