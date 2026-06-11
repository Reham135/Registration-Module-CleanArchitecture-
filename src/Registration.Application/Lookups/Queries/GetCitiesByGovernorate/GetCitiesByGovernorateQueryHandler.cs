using AutoMapper;
using MediatR;
using Registration.Application.Common.Interfaces;
using Registration.Application.Lookups.DTOs;

namespace Registration.Application.Lookups.Queries.GetCitiesByGovernorate;

public class GetCitiesByGovernorateQueryHandler : IRequestHandler<GetCitiesByGovernorateQuery, List<CityDto>>
{
    private readonly ILookupRepository _lookupRepository;
    private readonly IMapper _mapper;

    public GetCitiesByGovernorateQueryHandler(ILookupRepository lookupRepository, IMapper mapper)
    {
        _lookupRepository = lookupRepository;
        _mapper = mapper;
    }

    public async Task<List<CityDto>> Handle(GetCitiesByGovernorateQuery request, CancellationToken cancellationToken)
    {
        var cities = await _lookupRepository.GetActiveCitiesByGovernorateAsync(request.GovernorateId, cancellationToken);
        return _mapper.Map<List<CityDto>>(cities);
    }
}
