namespace SolarCharge.API.Application.Features.ChatBot.Domain;

public enum ChatMessageType
{
    Unknown = 0,
    StartCharging = 1,
    StopCharging = 2,
    InferredCharging = 3,
    InferredNotCharging = 4
}