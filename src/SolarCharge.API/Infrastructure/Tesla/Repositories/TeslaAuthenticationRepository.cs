using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Repositories;
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
    public async Task<Application.Models.TeslaAuthentication?> GetAsync()
    {
        logger.LogTrace("Retrieving Tesla Authentication");

        var encryptedTeslaAuthentication = await dbContext.TeslaAuthentications.SingleOrDefaultAsync();
        if (encryptedTeslaAuthentication is null)
        {
            logger.LogDebug("Tesla Authentication not found");
            return null;
        }
        
        return new Application.Models.TeslaAuthentication(
            AesEncryption.Decrypt(encryptedTeslaAuthentication.EncryptedAccessToken, teslaOptions.Value.EncryptionKey),
            AesEncryption.Decrypt(encryptedTeslaAuthentication.EncryptedRefreshToken, teslaOptions.Value.EncryptionKey));
    }

    public async Task SetAsync(Application.Models.TeslaAuthentication teslaAuthentication)
    {
        logger.LogTrace("Saving Tesla Authentication");

        var encryptedTeslaAuthentication = new TeslaAuthenticationEntity(
            AesEncryption.Encrypt(teslaAuthentication.AccessToken, teslaOptions.Value.EncryptionKey),
            AesEncryption.Encrypt(teslaAuthentication.RefreshToken, teslaOptions.Value.EncryptionKey));

        var existingTeslaAuthentication = await dbContext.TeslaAuthentications.SingleOrDefaultAsync();
        
        // Remove existing authentication properties if there is already data stored
        if (existingTeslaAuthentication is not null)
        {
            dbContext.Remove(existingTeslaAuthentication);
        }

        await dbContext.AddAsync(encryptedTeslaAuthentication);
        await dbContext.SaveChangesAsync();
    }
}