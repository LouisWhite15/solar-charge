namespace SolarCharge.API.Application.Interfaces;

public interface IChatBot
{
    Task SendMessage(string messageText);
}