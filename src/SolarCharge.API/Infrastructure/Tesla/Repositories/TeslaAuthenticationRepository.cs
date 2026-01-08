using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Features.TeslaAuth.Domain;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Infrastructure.DataAccess;
using SolarCharge.API.Infrastructure.DataAccess.Crypto;
using SolarCharge.API.Infrastructure.DataAccess.Entities;

namespace SolarCharge.API.Infrastructure.Tesla.Repositories;

public class TeslaAuthenticationRepository(
    ILogger<TeslaAuthenticationRepository> logger,
    ApplicationContext dbContext,
    IOptions<TeslaOptions> teslaOptions)
    : ITeslaAuthenticationRepository
{
    public async ValueTask<TeslaAuthentication?> GetAsync(CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Retrieving Tesla Authentication");

        var encryptedTeslaAuthentication = await dbContext.TeslaAuthentications.SingleOrDefaultAsync(cancellationToken);
        if (encryptedTeslaAuthentication is null)
        {
            logger.LogDebug("Tesla Authentication not found");
            return null;
        }
        
        return new TeslaAuthentication(
            AesEncryption.Decrypt(encryptedTeslaAuthentication.EncryptedAccessToken, teslaOptions.Value.EncryptionKey),
            AesEncryption.Decrypt(encryptedTeslaAuthentication.EncryptedRefreshToken, teslaOptions.Value.EncryptionKey));
    }

    public async ValueTask SetAsync(TeslaAuthentication teslaAuthentication, CancellationToken cancellationToken = default)
    {
        logger.LogTrace("Saving Tesla Authentication");

        var encryptedTeslaAuthentication = new TeslaAuthenticationEntity(
            AesEncryption.Encrypt(teslaAuthentication.AccessToken, teslaOptions.Value.EncryptionKey),
            AesEncryption.Encrypt(teslaAuthentication.RefreshToken, teslaOptions.Value.EncryptionKey));

        var existingTeslaAuthentication = await dbContext.TeslaAuthentications.SingleOrDefaultAsync(cancellationToken);
        
        // Remove existing authentication properties if there is already data stored
        if (existingTeslaAuthentication is not null)
        {
            dbContext.Remove(existingTeslaAuthentication);
        }

        await dbContext.AddAsync(encryptedTeslaAuthentication, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}