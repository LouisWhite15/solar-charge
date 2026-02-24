using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.ChatBot.Infrastructure;
using SolarCharge.API.Application.Shared;
using Wolverine;

namespace SolarCharge.API.Application.Features.ChatBot.Commands;

public sealed record SendChatMessageCommand(ChatMessageType Type, string MessageText)
{
    public class Handler(
        ILogger<Handler> logger,
        IChatBotClient chatBotClient) 
        : IWolverineHandler
    {
        public async Task HandleAsync(SendChatMessageCommand command, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending chat message. ChatMessageType: {ChatMessageType}", command.Type);
            await chatBotClient.SendMessageAsync(command.MessageText, cancellationToken);
        }
    }
}