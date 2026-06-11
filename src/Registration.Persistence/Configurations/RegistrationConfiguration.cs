using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Registration.Domain.ValueObjects;

namespace Registration.Persistence.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<Domain.Entities.Registration>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Registration> builder)
    {
        builder.ToTable("Registrations");

        builder.HasKey(r => r.Id);

        // --- Name value objects (owned, stored as sibling columns on the Registrations table) ---
        builder.OwnsOne(r => r.FirstName, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("FirstName")
                .HasMaxLength(PersonName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(r => r.MiddleName, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("MiddleName")
                .HasMaxLength(PersonName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(r => r.LastName, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("LastName")
                .HasMaxLength(PersonName.MaxLength)
                .IsRequired();
        });

        builder.Property(r => r.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        // --- MobileNumber value object: unique index on the normalized (E.164) value ---
        builder.OwnsOne(r => r.MobileNumber, mobile =>
        {
            mobile.Property(m => m.Value)
                .HasColumnName("MobileNumber")
                .HasMaxLength(20)
                .IsRequired();

            mobile.HasIndex(m => m.Value)
                .IsUnique()
                .HasDatabaseName("UX_Registrations_MobileNumber");
        });

        // --- Email value object: stores both the original value and a normalized
        //     (lowercased) value used for the case-insensitive uniqueness constraint ---
        builder.OwnsOne(r => r.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();

            email.Property(e => e.NormalizedValue)
                .HasColumnName("NormalizedEmail")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();

            email.HasIndex(e => e.NormalizedValue)
                .IsUnique()
                .HasDatabaseName("UX_Registrations_NormalizedEmail");
        });

        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.Property(r => r.CreatedBy).HasMaxLength(256);
        builder.Property(r => r.UpdatedBy).HasMaxLength(256);

        builder.HasMany(r => r.Addresses)
            .WithOne()
            .HasForeignKey(a => a.RegistrationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Addresses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(r => r.FirstName).IsRequired();
        builder.Navigation(r => r.LastName).IsRequired();
        builder.Navigation(r => r.MobileNumber).IsRequired();
        builder.Navigation(r => r.Email).IsRequired();
    }
}
