using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarCharge.API.Application.Features.Vehicles.Domain;

namespace SolarCharge.API.Infrastructure.Database.EntityTypeConfigurations;

public class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.DisplayName).IsRequired().HasMaxLength(DatabaseConstants.MaxStringLength);
        builder.Property(v => v.State).IsRequired().HasConversion<string>();
        builder.Property(v => v.LastUpdated).IsRequired();
        builder.Property(v => v.IsCharging).IsRequired();
    }
}