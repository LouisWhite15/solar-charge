using SolarCharge.ChatBot.Telegram;

namespace SolarCharge.ChatBot.Modules;

public static class TelegramExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(
            configuration.GetSection(TelegramOptions.Telegram));
        
        services.AddHostedService<TelegramBot>();
        
        return services;
    }
}