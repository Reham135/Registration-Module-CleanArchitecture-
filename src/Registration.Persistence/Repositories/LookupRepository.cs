using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Persistence.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly ApplicationDbContext _context;

    public LookupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> GovernorateExistsAsync(int governorateId, CancellationToken cancellationToken = default)
    {
        return _context.Governorates
            .AsNoTracking()
            .AnyAsync(g => g.Id == governorateId && g.IsActive, cancellationToken);
    }

    public Task<bool> CityBelongsToGovernorateAsync(int cityId, int governorateId, CancellationToken cancellationToken = default)
    {
        return _context.Cities
            .AsNoTracking()
            .AnyAsync(c => c.Id == cityId && c.GovernorateId == governorateId && c.IsActive, cancellationToken);
    }

    public Task<List<Governorate>> GetActiveGovernoratesAsync(CancellationToken cancellationToken = default)
    {
        return _context.Governorates
            .AsNoTracking()
            .Where(g => g.IsActive)
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<City>> GetActiveCitiesByGovernorateAsync(int governorateId, CancellationToken cancellationToken = default)
    {
        return _context.Cities
            .AsNoTracking()
            .Where(c => c.GovernorateId == governorateId && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}
