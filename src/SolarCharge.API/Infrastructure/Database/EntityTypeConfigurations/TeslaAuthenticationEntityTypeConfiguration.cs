using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarCharge.API.Infrastructure.Database.Entities;

namespace SolarCharge.API.Infrastructure.Database.EntityTypeConfigurations;

public class TeslaAuthenticationEntityTypeConfiguration : IEntityTypeConfiguration<TeslaAuthenticationEntity>
{
    public void Configure(EntityTypeBuilder<TeslaAuthenticationEntity> builder)
    {
        builder.HasKey(t => t.Id);
    }
}