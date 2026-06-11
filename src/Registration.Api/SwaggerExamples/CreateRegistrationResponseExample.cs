using Registration.Api.Controllers;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.SwaggerExamples;

public class CreateRegistrationResponseExample : IExamplesProvider<CreateRegistrationResponse>
{
    public CreateRegistrationResponse GetExamples() => new(1);
}
