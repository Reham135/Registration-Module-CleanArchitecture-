using Registration.Domain.Entities;

namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Read-side repository for address lookup data (governorates and cities).
/// </summary>
public interface ILookupRepository
{
    Task<bool> GovernorateExistsAsync(int governorateId, CancellationToken cancellationToken = default);

    Task<bool> CityBelongsToGovernorateAsync(int cityId, int governorateId, CancellationToken cancellationToken = default);

    Task<List<Governorate>> GetActiveGovernoratesAsync(CancellationToken cancellationToken = default);

    Task<List<City>> GetActiveCitiesByGovernorateAsync(int governorateId, CancellationToken cancellationToken = default);
}
