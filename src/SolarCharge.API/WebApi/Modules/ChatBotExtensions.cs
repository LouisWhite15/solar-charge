using SolarCharge.API.Application.Features;
using SolarCharge.API.Application.Interfaces;
using SolarCharge.API.Infrastructure.ChatBot;

namespace SolarCharge.API.WebApi.Modules;

public static class ChatBotExtensions
{
    public static IServiceCollection AddChatBot(this IServiceCollection services, IConfiguration configuration)
    {
        var featureOptions = configuration.GetSection(FeatureOptions.Features).Get<FeatureOptions>();
        
        services.Configure<ChatBotOptions>(
            configuration.GetSection(ChatBotOptions.ChatBot));

        if (featureOptions?.IsChatBotEnabled is true)
        {
            services.AddScoped<IChatBot, ChatBotService>();
        }
        else
        {
            services.AddScoped<IChatBot, NoOpChatBotService>();
        }

        return services;
    }
}