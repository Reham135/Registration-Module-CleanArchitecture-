using Microsoft.EntityFrameworkCore;
using Registration.Domain.Entities;

namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext, exposed to the Application layer for
/// read-only (AsNoTracking) query handlers. Pragmatic Clean Architecture exception
/// (à la Jason Taylor template) — write paths go through dedicated repositories.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Registration> Registrations { get; }

    DbSet<Address> Addresses { get; }

    DbSet<Governorate> Governorates { get; }

    DbSet<City> Cities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
