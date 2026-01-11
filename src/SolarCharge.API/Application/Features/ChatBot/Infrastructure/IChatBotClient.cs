namespace SolarCharge.API.Application.Features.ChatBot.Infrastructure;

public interface IChatBotClient
{
    Task SendMessageAsync(string messageText, CancellationToken cancellationToken = default);
}