using Registration.Application.Registrations.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.SwaggerExamples;

public class RegistrationDtoExample : IExamplesProvider<RegistrationDto>
{
    public RegistrationDto GetExamples()
    {
        return new RegistrationDto
        {
            Id = 1,
            FirstName = "Ahmed",
            MiddleName = "Mohamed",
            LastName = "Ali",
            BirthDate = new DateOnly(1990, 5, 15),
            MobileNumber = "+201006158123",
            Email = "ahmed.ali@example.com",
            Addresses = new List<AddressDto>
            {
                new()
                {
                    Id = 1,
                    GovernorateId = 1,
                    GovernorateName = "Cairo",
                    CityId = 101,
                    CityName = "Nasr City",
                    Street = "10 Tahrir Street",
                    BuildingNumber = "12A",
                    FlatNumber = "3",
                    IsPrimary = true,
                },
            },
        };
    }
}
