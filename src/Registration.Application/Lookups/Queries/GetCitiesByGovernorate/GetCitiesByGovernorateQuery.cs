using MediatR;
using Registration.Application.Lookups.DTOs;

namespace Registration.Application.Lookups.Queries.GetCitiesByGovernorate;

/// <summary>
/// Returns all active cities belonging to the specified governorate.
/// </summary>
public record GetCitiesByGovernorateQuery(int GovernorateId) : IRequest<List<CityDto>>;
