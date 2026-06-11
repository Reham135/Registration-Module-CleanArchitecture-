using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Registration.Domain.Entities;

namespace Registration.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(Address.StreetMaxLength);

        builder.Property(a => a.BuildingNumber)
            .IsRequired()
            .HasMaxLength(Address.BuildingFlatMaxLength);

        builder.Property(a => a.FlatNumber)
            .IsRequired()
            .HasMaxLength(Address.BuildingFlatMaxLength);

        builder.Property(a => a.IsPrimary)
            .IsRequired();

        builder.Property(a => a.CreatedAtUtc).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(256);
        builder.Property(a => a.UpdatedBy).HasMaxLength(256);

        builder.HasIndex(a => a.RegistrationId);

        // Lookup FKs are restricted: governorates/cities are reference data and must
        // not be deletable while referenced by an address.
        builder.HasOne(a => a.Governorate)
            .WithMany()
            .HasForeignKey(a => a.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.GovernorateId);
        builder.HasIndex(a => a.CityId);
    }
}
