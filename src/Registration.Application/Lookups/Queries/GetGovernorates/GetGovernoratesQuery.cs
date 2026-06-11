using MediatR;
using Registration.Application.Lookups.DTOs;

namespace Registration.Application.Lookups.Queries.GetGovernorates;

/// <summary>
/// Returns all active governorates.
/// </summary>
public record GetGovernoratesQuery : IRequest<List<GovernorateDto>>;
