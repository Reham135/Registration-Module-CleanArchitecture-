using Registration.Application.Registrations.Commands.CreateRegistration;
using Registration.Application.Registrations.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.SwaggerExamples;

public class CreateRegistrationCommandExample : IExamplesProvider<CreateRegistrationCommand>
{
    public CreateRegistrationCommand GetExamples()
    {
        return new CreateRegistrationCommand
        {
            FirstName = "Ahmed",
            MiddleName = "Mohamed",
            LastName = "Ali",
            BirthDate = new DateOnly(1990, 5, 15),
            MobileNumber = "+201006158123",
            Email = "ahmed.ali@example.com",
            Addresses = new List<CreateAddressDto>
            {
                new()
                {
                    GovernorateId = 1,
                    CityId = 101,
                    Street = "10 Tahrir Street",
                    BuildingNumber = "12A",
                    FlatNumber = "3",
                    IsPrimary = true,
                },
            },
        };
    }
}
