using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SolarCharge.ChatBot.Telegram;

public class TelegramBot : BackgroundService
{
    private readonly ILogger<TelegramBot> _logger;
    
    private readonly string _botToken;
    private long _chatId = 0;
    
    public TelegramBot(ILogger<TelegramBot> logger, IOptions<TelegramOptions> options)
    {
        _logger = logger;
        _botToken = options.Value.BotToken;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = new TelegramBotClient(_botToken, cancellationToken: stoppingToken);
        var me = await bot.GetMe(stoppingToken);
        bot.OnError += OnError;
        bot.OnMessage += OnMessage;
        
        _logger.LogInformation("{BotUsername} is running!", me.Username);
    }

    private Task OnError(Exception exception, HandleErrorSource source)
    {
        _logger.LogError(exception, "An error occurred with {Source}", source);
        return Task.CompletedTask;
    }

    private Task OnMessage(Message message, UpdateType type)
    {
        _logger.LogInformation("Using ChatId: {ChatId} for future communications", message.Chat.Id);
        
        _chatId =  message.Chat.Id;
        
        return Task.CompletedTask;
    }
}