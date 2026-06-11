using AutoMapper;
using MediatR;
using Registration.Application.Common.Interfaces;
using Registration.Application.Lookups.DTOs;

namespace Registration.Application.Lookups.Queries.GetGovernorates;

public class GetGovernoratesQueryHandler : IRequestHandler<GetGovernoratesQuery, List<GovernorateDto>>
{
    private readonly ILookupRepository _lookupRepository;
    private readonly IMapper _mapper;

    public GetGovernoratesQueryHandler(ILookupRepository lookupRepository, IMapper mapper)
    {
        _lookupRepository = lookupRepository;
        _mapper = mapper;
    }

    public async Task<List<GovernorateDto>> Handle(GetGovernoratesQuery request, CancellationToken cancellationToken)
    {
        var governorates = await _lookupRepository.GetActiveGovernoratesAsync(cancellationToken);
        return _mapper.Map<List<GovernorateDto>>(governorates);
    }
}
