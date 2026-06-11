using System.Text.RegularExpressions;
using Registration.Domain.Common;
using Registration.Domain.Exceptions;

namespace Registration.Domain.Entities;

/// <summary>
/// Represents a postal address belonging to a <see cref="Registration"/>.
/// </summary>
public partial class Address : BaseAuditableEntity
{
    public const int StreetMaxLength = 200;
    public const int BuildingFlatMaxLength = 20;

    public int RegistrationId { get; private set; }

    public int GovernorateId { get; private set; }

    public int CityId { get; private set; }

    public string Street { get; private set; } = null!;

    public string BuildingNumber { get; private set; } = null!;

    public string FlatNumber { get; private set; } = null!;

    public bool IsPrimary { get; internal set; }

    /// <summary>
    /// Read-only navigation for mapping convenience (e.g., to expose Governorate/City names in DTOs).
    /// </summary>
    public Governorate? Governorate { get; private set; }

    /// <summary>
    /// Read-only navigation for mapping convenience (e.g., to expose Governorate/City names in DTOs).
    /// </summary>
    public City? City { get; private set; }

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// </summary>
    protected Address()
    {
    }

    private Address(int governorateId, int cityId, string street, string buildingNumber, string flatNumber, bool isPrimary)
    {
        GovernorateId = governorateId;
        CityId = cityId;
        Street = street;
        BuildingNumber = buildingNumber;
        FlatNumber = flatNumber;
        IsPrimary = isPrimary;
    }

    public static Address Create(int governorateId, int cityId, string? street, string? buildingNumber, string? flatNumber, bool isPrimary)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new DomainException("Street is required.");
        }

        var trimmedStreet = street.Trim();
        if (trimmedStreet.Length > StreetMaxLength)
        {
            throw new DomainException($"Street must not exceed {StreetMaxLength} characters.");
        }

        var validatedBuilding = ValidateBuildingFlat(buildingNumber, "Building number");
        var validatedFlat = ValidateBuildingFlat(flatNumber, "Flat number");

        return new Address(governorateId, cityId, trimmedStreet, validatedBuilding, validatedFlat, isPrimary);
    }

    private static string ValidateBuildingFlat(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{fieldName} is required.");
        }

        var trimmed = value.Trim();

        if (trimmed.Length > BuildingFlatMaxLength)
        {
            throw new DomainException($"{fieldName} must not exceed {BuildingFlatMaxLength} characters.");
        }

        if (!BuildingFlatRegex().IsMatch(trimmed))
        {
            throw new DomainException($"{fieldName} contains invalid characters.");
        }

        return trimmed;
    }

    /// <summary>
    /// Allows alphanumeric characters, Arabic letters, slashes, hyphens and spaces.
    /// </summary>
    [GeneratedRegex(@"^[A-Za-z0-9؀-ۿ\/\-\s]+$")]
    private static partial Regex BuildingFlatRegex();

    internal void AssignToRegistration(int registrationId)
    {
        RegistrationId = registrationId;
    }

    internal void SetAsPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
    }
}
