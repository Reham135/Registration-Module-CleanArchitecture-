namespace Registration.Application.Registrations.DTOs;

public class RegistrationDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string MobileNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public List<AddressDto> Addresses { get; set; } = new();
}
