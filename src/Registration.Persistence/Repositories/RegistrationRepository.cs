using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Persistence.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly ApplicationDbContext _context;

    public RegistrationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return _context.Registrations
            .AsNoTracking()
            .AnyAsync(r => r.Email.NormalizedValue == normalizedEmail, cancellationToken);
    }

    public Task<bool> ExistsByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        return _context.Registrations
            .AsNoTracking()
            .AnyAsync(r => r.MobileNumber.Value == mobileNumber, cancellationToken);
    }

    public async Task AddAsync(Domain.Entities.Registration registration, CancellationToken cancellationToken = default)
    {
        await _context.Registrations.AddAsync(registration, cancellationToken);
    }

    public Task<Domain.Entities.Registration?> GetByIdWithAddressesAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Registrations
            .Include(r => r.Addresses)
                .ThenInclude(a => a.Governorate)
            .Include(r => r.Addresses)
                .ThenInclude(a => a.City)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
