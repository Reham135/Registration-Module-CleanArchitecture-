using AutoMapper;
using Registration.Application.Lookups.DTOs;
using Registration.Application.Registrations.DTOs;
using Registration.Domain.Entities;

namespace Registration.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile mapping domain entities (and their value objects) to DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Registration, RegistrationDto>()
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName.Value))
            .ForMember(d => d.MiddleName, opt => opt.MapFrom(s => s.MiddleName != null ? s.MiddleName.Value : null))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName.Value))
            .ForMember(d => d.MobileNumber, opt => opt.MapFrom(s => s.MobileNumber.Value))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email.Value))
            .ForMember(d => d.Addresses, opt => opt.MapFrom(s => s.Addresses));

        CreateMap<Address, AddressDto>()
            .ForMember(d => d.GovernorateName, opt => opt.MapFrom(s => s.Governorate != null ? s.Governorate.Name : string.Empty))
            .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City != null ? s.City.Name : string.Empty));

        CreateMap<Governorate, GovernorateDto>();

        CreateMap<City, CityDto>();
    }
}
