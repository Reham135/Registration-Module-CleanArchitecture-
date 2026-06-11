namespace Registration.Application.Registrations.DTOs;

/// <summary>
/// Represents an address supplied as part of a <see cref="Commands.CreateRegistration.CreateRegistrationCommand"/>.
/// </summary>
public class CreateAddressDto
{
    public int GovernorateId { get; set; }

    public int CityId { get; set; }

    public string Street { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string FlatNumber { get; set; } = null!;

    public bool IsPrimary { get; set; }
}
