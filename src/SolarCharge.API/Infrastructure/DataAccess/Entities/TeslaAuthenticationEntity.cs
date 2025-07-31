using System.ComponentModel.DataAnnotations;

namespace SolarCharge.API.Infrastructure.DataAccess.Entities;

public sealed record TeslaAuthenticationEntity(string EncryptedAccessToken, string EncryptedRefreshToken)
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
}
