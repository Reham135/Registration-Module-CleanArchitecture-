namespace Registration.Application.Registrations.DTOs;

public class AddressDto
{
    public int Id { get; set; }

    public int GovernorateId { get; set; }

    public string GovernorateName { get; set; } = null!;

    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string FlatNumber { get; set; } = null!;

    public bool IsPrimary { get; set; }
}
