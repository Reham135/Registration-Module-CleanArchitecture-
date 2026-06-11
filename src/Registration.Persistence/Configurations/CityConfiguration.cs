using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Registration.Domain.Entities;
using Registration.Persistence.Seed;

namespace Registration.Persistence.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities");

        builder.HasKey(c => c.Id);

        // Fixed ids are seeded explicitly; do not use identity columns for lookup data.
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.HasIndex(c => c.GovernorateId);

        builder.HasIndex(c => new { c.GovernorateId, c.Name }).IsUnique();

        builder.HasData(SeedData.Cities);
    }
}
