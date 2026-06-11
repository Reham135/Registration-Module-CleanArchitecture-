using Registration.Application.Lookups.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.SwaggerExamples;

public class GovernorateListExample : IExamplesProvider<List<GovernorateDto>>
{
    public List<GovernorateDto> GetExamples() => new()
    {
        new GovernorateDto { Id = 1, Name = "Cairo" },
        new GovernorateDto { Id = 2, Name = "Giza" },
    };
}

public class CityListExample : IExamplesProvider<List<CityDto>>
{
    public List<CityDto> GetExamples() => new()
    {
        new CityDto { Id = 101, Name = "Nasr City", GovernorateId = 1 },
        new CityDto { Id = 102, Name = "Maadi", GovernorateId = 1 },
    };
}
