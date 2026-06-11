using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Registration.Domain.Entities;
using Registration.Persistence.Seed;

namespace Registration.Persistence.Configurations;

public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
{
    public void Configure(EntityTypeBuilder<Governorate> builder)
    {
        builder.ToTable("Governorates");

        builder.HasKey(g => g.Id);

        // Fixed ids are seeded explicitly; do not use identity columns for lookup data.
        builder.Property(g => g.Id).ValueGeneratedNever();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.IsActive)
            .IsRequired();

        builder.HasIndex(g => g.Name).IsUnique();

        builder.HasMany(g => g.Cities)
            .WithOne(c => c.Governorate)
            .HasForeignKey(c => c.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(SeedData.Governorates);
    }
}
