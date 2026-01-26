namespace SolarCharge.API.Infrastructure.Database.Entities;

public sealed record TeslaAuthenticationEntity(string EncryptedAccessToken, string EncryptedRefreshToken)
{
    public Guid Id { get; init; } = Guid.NewGuid();
}