using SolarCharge.API.Application.Features.ChatBot.Infrastructure;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class NoOpChatBotClient(ILogger<NoOpChatBotClient> logger) : IChatBotClient
{
    public Task SendMessageAsync(string messageText, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("ChatBot integration is disabled. Not sending message");
        return Task.CompletedTask;
    }
}