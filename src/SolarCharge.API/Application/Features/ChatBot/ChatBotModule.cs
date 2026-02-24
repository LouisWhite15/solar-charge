using SolarCharge.API.Application.Features.ChatBot.Infrastructure;
using SolarCharge.API.Infrastructure.ChatBot;

namespace SolarCharge.API.Application.Features.ChatBot;

public static class ChatBotModule
{
    public static IServiceCollection AddChatBot(this IServiceCollection services, IConfiguration configuration)
    {
        var featureOptions = configuration.GetSection(FeatureOptions.SectionName).Get<FeatureOptions>();
        
        services.Configure<ChatBotOptions>(
            configuration.GetSection(ChatBotOptions.SectionName));

        services.AddSingleton<ILastChatMessageCache, LastChatMessageCache>();

        if (featureOptions?.IsChatBotEnabled is true)
        {
            services.AddTransient<IChatBotClient, ChatBotClient>();
        }
        else
        {
            services.AddTransient<IChatBotClient, NoOpChatBotClient>();
        }

        return services;
    }
}