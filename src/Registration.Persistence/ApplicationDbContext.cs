using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.Models;
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

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
