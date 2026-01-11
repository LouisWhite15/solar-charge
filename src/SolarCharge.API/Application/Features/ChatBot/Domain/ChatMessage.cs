namespace SolarCharge.API.Application.Features.ChatBot.Domain;

public sealed record ChatMessage(ChatMessageType Type, DateTimeOffset Timestamp);
