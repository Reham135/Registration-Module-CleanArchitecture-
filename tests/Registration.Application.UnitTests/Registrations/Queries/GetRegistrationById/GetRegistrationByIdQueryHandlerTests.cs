using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Registration.Application.Common.Exceptions;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.Mappings;
using Registration.Application.Registrations.Queries.GetRegistrationById;
using Registration.Domain.Entities;
using Xunit;

namespace Registration.Application.UnitTests.Registrations.Queries.GetRegistrationById;

/// <summary>
/// In-memory <see cref="IApplicationDbContext"/> stub backed by EF Core's InMemory provider,
/// used purely to exercise the query handler's mapping/Include logic and not-found behaviour.
/// </summary>
public class GetRegistrationByIdQueryHandlerTests
{
    private static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        return configuration.CreateMapper();
    }

    private static TestDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new TestDbContext(options);
    }

    [Fact]
    public async Task Handle_WhenRegistrationExists_ReturnsMappedDto()
    {
        await using var context = CreateContext(nameof(Handle_WhenRegistrationExists_ReturnsMappedDto));

        var governorate = new Governorate(1, "Cairo");
        var city = new City(101, "Nasr City", governorateId: 1);
        context.Governorates.Add(governorate);
        context.Cities.Add(city);

        var address = Address.Create(1, 101, "10 Tahrir Street", "12A", "3", true);
        var registration = Domain.Entities.Registration.Create(
            "Ahmed", "Mohamed", "Ali",
            new DateOnly(1990, 5, 15),
            "+201006158123",
            "ahmed.ali@example.com",
            new[] { address },
            new DateOnly(2026, 6, 10));

        context.Registrations.Add(registration);
        await context.SaveChangesAsync();

        var handler = new GetRegistrationByIdQueryHandler(context, CreateMapper());

        var result = await handler.Handle(new GetRegistrationByIdQuery(registration.Id), CancellationToken.None);

        result.FirstName.Should().Be("Ahmed");
        result.Email.Should().Be("ahmed.ali@example.com");
        result.Addresses.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_WhenRegistrationDoesNotExist_ThrowsNotFoundException()
    {
        await using var context = CreateContext(nameof(Handle_WhenRegistrationDoesNotExist_ThrowsNotFoundException));

        var handler = new GetRegistrationByIdQueryHandler(context, CreateMapper());

        var act = () => handler.Handle(new GetRegistrationByIdQuery(999), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

/// <summary>
/// Minimal DbContext implementation for use with EF Core's InMemory provider in handler tests.
/// </summary>
public class TestDbContext : DbContext, IApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Registration> Registrations => Set<Domain.Entities.Registration>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Governorate> Governorates => Set<Governorate>();

    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Registration>(builder =>
        {
            builder.OwnsOne(r => r.FirstName, (OwnedNavigationBuilder<Domain.Entities.Registration, Domain.ValueObjects.PersonName> n) =>
                n.Property(p => p.Value));
            builder.OwnsOne(r => r.MiddleName, (OwnedNavigationBuilder<Domain.Entities.Registration, Domain.ValueObjects.PersonName> n) =>
                n.Property(p => p.Value));
            builder.OwnsOne(r => r.LastName, (OwnedNavigationBuilder<Domain.Entities.Registration, Domain.ValueObjects.PersonName> n) =>
                n.Property(p => p.Value));
            builder.OwnsOne(r => r.MobileNumber, (OwnedNavigationBuilder<Domain.Entities.Registration, Domain.ValueObjects.MobileNumber> m) =>
                m.Property(p => p.Value));
            builder.OwnsOne(r => r.Email, (OwnedNavigationBuilder<Domain.Entities.Registration, Domain.ValueObjects.Email> e) =>
            {
                e.Property(p => p.Value);
                e.Property(p => p.NormalizedValue);
            });

            builder.HasMany(r => r.Addresses)
                .WithOne()
                .HasForeignKey(a => a.RegistrationId);

            builder.Navigation(r => r.Addresses).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Address>(builder =>
        {
            builder.HasOne(a => a.Governorate).WithMany().HasForeignKey(a => a.GovernorateId);
            builder.HasOne(a => a.City).WithMany().HasForeignKey(a => a.CityId);
        });
    }
}
