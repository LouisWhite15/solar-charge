using Microsoft.Extensions.Options;
using SolarCharge.ChatBot.Infrastructure.DataAccess;
using Telegram.Bot;

namespace SolarCharge.ChatBot.Telegram;

public interface ITelegramService
{
    Task SendMessageAsync(string messageText);
}

public class TelegramService(
    ILogger<TelegramService> logger,
    IOptions<TelegramOptions> options,
    BotContext botContext)
    : ITelegramService
{
    private readonly string _botToken = options.Value.BotToken;

    public async Task SendMessageAsync(string messageText)
    {
        var chatId = botContext.Bots.SingleOrDefault()?.ChatId;
        if (chatId is null)
        {
            logger.LogWarning("ChatId not found, not sending message");
            return;
        }
        
        var bot = new TelegramBotClient(_botToken);
        await bot.SendMessage(chatId,  messageText);
    }
}