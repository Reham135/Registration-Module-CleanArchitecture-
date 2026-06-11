using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Registration.Application.Common.Exceptions;
using Registration.Application.Common.Interfaces;
using Registration.Application.Registrations.DTOs;

namespace Registration.Application.Registrations.Queries.GetRegistrationById;

public class GetRegistrationByIdQueryHandler : IRequestHandler<GetRegistrationByIdQuery, RegistrationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRegistrationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegistrationDto> Handle(GetRegistrationByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await _context.Registrations
            .AsNoTracking()
            .Include(r => r.Addresses)
                .ThenInclude(a => a.Governorate)
            .Include(r => r.Addresses)
                .ThenInclude(a => a.City)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (registration is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Registration), request.Id);
        }

        return _mapper.Map<RegistrationDto>(registration);
    }
}
