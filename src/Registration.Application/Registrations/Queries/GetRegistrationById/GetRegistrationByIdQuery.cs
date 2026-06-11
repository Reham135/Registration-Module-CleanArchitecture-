using MediatR;
using Registration.Application.Registrations.DTOs;

namespace Registration.Application.Registrations.Queries.GetRegistrationById;

/// <summary>
/// Returns a single registration (with its addresses and address lookup names) by id.
/// Throws <see cref="Common.Exceptions.NotFoundException"/> if not found.
/// </summary>
public record GetRegistrationByIdQuery(int Id) : IRequest<RegistrationDto>;
