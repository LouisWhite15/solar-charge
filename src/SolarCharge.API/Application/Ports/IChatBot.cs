namespace SolarCharge.API.Application.Ports;

public interface IChatBot
{
    Task SendMessage(string messageText);
}