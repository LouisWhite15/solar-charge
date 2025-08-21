using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Infrastructure.ChatBot;

namespace SolarCharge.API.Api.Modules;

public static class ChatBotExtensions
{
    public static IServiceCollection AddChatBot(this IServiceCollection services, IConfiguration configuration)
    {
        var featureOptions = configuration.GetSection(FeatureOptions.Features).Get<FeatureOptions>();
        
        services.Configure<ChatBotOptions>(
            configuration.GetSection(ChatBotOptions.ChatBot));

        if (featureOptions?.IsChatBotEnabled is true)
        {
            services.AddTransient<IChatBot, ChatBotService>();
        }
        else
        {
            services.AddTransient<IChatBot, NoOpChatBotService>();
        }

        return services;
    }
}