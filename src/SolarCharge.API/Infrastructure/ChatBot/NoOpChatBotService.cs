using SolarCharge.API.Application.Interfaces;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class NoOpChatBotService(ILogger<NoOpChatBotService> logger) : IChatBot
{
    public Task SendMessage(string messageText)
    {
        logger.LogDebug("ChatBot integration is disabled. Not sending message");
        return Task.CompletedTask;
    }
}