using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarCharge.ChatBot.Infrastructure.DataAccess;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SolarCharge.ChatBot.Telegram;

public class TelegramBot(
    ILogger<TelegramBot> logger,
    IOptions<TelegramOptions> options,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly string _botToken = options.Value.BotToken;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = new TelegramBotClient(_botToken, cancellationToken: stoppingToken);
        var me = await bot.GetMe(stoppingToken);
        bot.OnError += OnError;
        bot.OnMessage += OnMessage;
        
        logger.LogInformation("{BotUsername} is running!", me.Username);
    }

    private Task OnError(Exception exception, HandleErrorSource source)
    {
        logger.LogError(exception, "An error occurred with {Source}", source);
        return Task.CompletedTask;
    }

    private async Task OnMessage(Message message, UpdateType type)
    {
        if (message.Text?.ToLower() == "/start")
        {
            await ExecuteStartCommandAsync(message.Chat.Id);
        }
    }

    private async Task ExecuteStartCommandAsync(long chatId)
    {
        logger.LogInformation("Using ChatId: {ChatId} for future communications", chatId);

        using var scope = serviceScopeFactory.CreateScope();
        var botContext = scope.ServiceProvider.GetRequiredService<BotContext>();
        var existingBot = await botContext.Bots.SingleOrDefaultAsync();
        if (existingBot?.ChatId is null)
        {
            await SaveAsync(botContext, chatId);
            await SendMessage(chatId, "Solar Charge Telegram Bot setup is complete! This chat will be used for all future communications.");
            return;
        }
        
        if (existingBot.ChatId == chatId)
        {
            logger.LogDebug("ChatId is already set to this chat. Continuing to use ChatId: {ChatId}", chatId);
            await SendMessage(chatId, "Solar Charge Telegram Bot is already set up to use this chat!");
            return;
        }

        await UpdateAsync(botContext, chatId);
        await SendMessage(chatId, "Solar Charge Telegram Bot setup is complete! This chat will be used for all future communications");
    }

    private async Task SaveAsync(BotContext context, long chatId)
    {
        logger.LogInformation("Setting ChatId for future communication. ChatId: {ChatId}", chatId);
        
        await context.Bots.AddAsync(new BotEntity(chatId));
        await context.SaveChangesAsync();
    }

    private async Task UpdateAsync(BotContext context, long chatId)
    {
        logger.LogInformation("Updating ChatId for future communication. ChatId: {ChatId}", chatId);

        var bot = await context.Bots.SingleOrDefaultAsync();
        if (bot is null)
        {
            return;
        }
        
        bot.ChatId = chatId;
        await context.SaveChangesAsync();
    }

    private async Task SendMessage(long chatId, string message)
    {
        var bot = new TelegramBotClient(_botToken);
        await bot.SendMessage(chatId, message);
    }
}