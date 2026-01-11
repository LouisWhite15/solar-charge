using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.ChatBot.Infrastructure;
using SolarCharge.API.Application.Shared;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChatBot.Commands;

public sealed record SendChatMessageCommand(ChatMessageType Type, string MessageText)
{
    public class Handler(
        ILogger<Handler> logger,
        IChatBotClient chatBotClient,
        ILastChatMessageCache lastChatMessageCache,
        IClock clock) : IWolverineHandler
    {
        public async ValueTask HandleAsync(SendChatMessageCommand command, CancellationToken cancellationToken = default)
        {
            var lastChatMessage = await lastChatMessageCache.GetAsync(cancellationToken);

            if (lastChatMessage is not null && lastChatMessage.Type == command.Type)
            {
                logger.LogDebug("This chat message type was the last sent message. Skipping send. ChatMessageType: {ChatMessageType}. LastSentAt: {LastSentAt}", command.Type, lastChatMessage.Timestamp);
                return;
            }
            
            logger.LogInformation("Sending chat message. ChatMessageType: {ChatMessageType}", command.Type);
            await chatBotClient.SendMessageAsync(command.MessageText, cancellationToken);
            
            await lastChatMessageCache.SetAsync(new ChatMessage(command.Type, clock.UtcNow), cancellationToken);
        }
    }
}