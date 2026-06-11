using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Registration.Application.Common.Models;

namespace Registration.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(m => m.Content)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(m => m.Error)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(m => m.ProcessedOnUtc);
    }
}
