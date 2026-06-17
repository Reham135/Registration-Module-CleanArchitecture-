using MassTransit;
using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Registration> Registrations => Set<Domain.Entities.Registration>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Governorate> Governorates => Set<Governorate>();

    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Register MassTransit built-in outbox tables:
        //   - InboxState  : tracks consumed message state (deduplication)
        //   - OutboxMessage : stores pending outbound messages
        //   - OutboxState   : per-endpoint delivery state
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        base.OnModelCreating(modelBuilder);
    }
}
