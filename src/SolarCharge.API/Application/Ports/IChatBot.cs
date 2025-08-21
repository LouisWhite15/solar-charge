namespace SolarCharge.API.Application.Ports;

public interface IChatBot
{
    Task SendMessageAsync(string messageText);
}