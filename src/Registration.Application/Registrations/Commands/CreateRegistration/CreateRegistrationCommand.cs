using MediatR;
using Registration.Application.Registrations.DTOs;

namespace Registration.Application.Registrations.Commands.CreateRegistration;

/// <summary>
/// Command to register a new person along with 1-5 addresses.
/// Returns the newly created registration's id.
/// </summary>
public record CreateRegistrationCommand : IRequest<int>
{
    public string FirstName { get; init; } = null!;

    public string? MiddleName { get; init; }

    public string LastName { get; init; } = null!;

    public DateOnly BirthDate { get; init; }

    public string MobileNumber { get; init; } = null!;

    public string Email { get; init; } = null!;

    public List<CreateAddressDto> Addresses { get; init; } = new();
}
