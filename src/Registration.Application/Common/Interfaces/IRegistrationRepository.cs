namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Write-side repository for the Registration aggregate.
/// </summary>
public interface IRegistrationRepository
{
    Task<bool> ExistsByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task<bool> ExistsByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default);

    Task AddAsync(Domain.Entities.Registration registration, CancellationToken cancellationToken = default);

    Task<Domain.Entities.Registration?> GetByIdWithAddressesAsync(int id, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
