using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;

namespace SolarCharge.API.Application.Services;

public interface INotificationService
{
    Task SendAsync(NotificationType notificationType, params object[]? args);
}

public class NotificationService(
    ILogger<NotificationService> logger,
    IChatBot chatBot)
    : INotificationService
{
    private readonly Dictionary<NotificationType, string> _notificationMessages = new()
    {
        { NotificationType.StartCharging, "Start charging! Currently supplying {0}W to the grid" },
        { NotificationType.StopCharging, "Stop charging! Currently pulling {0}W from the grid" }
    };

    private NotificationType _lastSentNotificationType = NotificationType.Unknown;
    
    public async Task SendAsync(NotificationType notificationType, params object[]? args)
    {
        if (!_notificationMessages.TryGetValue(notificationType, out var message))
        {
            logger.LogWarning("No Notification found for the key {NotificationKey}", notificationType);
            return;
        }

        if (_lastSentNotificationType == notificationType)
        {
            logger.LogDebug("This notification was the last sent notification. Skipping send");
            return;
        }

        var formattedMessage = args is not null 
            ? string.Format(message, args)
            : message;
        
        logger.LogInformation("Sending notification. NotificationKey: {NotificationKey}", notificationType);
        await chatBot.SendMessageAsync(formattedMessage);
        
        _lastSentNotificationType = notificationType;
    }
}