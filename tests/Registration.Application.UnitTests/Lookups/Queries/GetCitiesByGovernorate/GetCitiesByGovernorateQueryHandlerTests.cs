using AutoMapper;
using FluentAssertions;
using Moq;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.Mappings;
using Registration.Application.Lookups.Queries.GetCitiesByGovernorate;
using Registration.Domain.Entities;
using Xunit;

namespace Registration.Application.UnitTests.Lookups.Queries.GetCitiesByGovernorate;

public class GetCitiesByGovernorateQueryHandlerTests
{
    private static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        return configuration.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsActiveCitiesForGovernorate()
    {
        var lookupRepositoryMock = new Mock<ILookupRepository>();
        var cities = new List<City>
        {
            new(101, "Nasr City", governorateId: 1),
            new(102, "Maadi", governorateId: 1),
        };

        lookupRepositoryMock
            .Setup(r => r.GetActiveCitiesByGovernorateAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cities);

        var handler = new GetCitiesByGovernorateQueryHandler(lookupRepositoryMock.Object, CreateMapper());

        var result = await handler.Handle(new GetCitiesByGovernorateQuery(1), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Nasr City" && c.GovernorateId == 1);
    }

    [Fact]
    public async Task Handle_WithNonExistentGovernorate_ReturnsEmptyList()
    {
        var lookupRepositoryMock = new Mock<ILookupRepository>();

        lookupRepositoryMock
            .Setup(r => r.GetActiveCitiesByGovernorateAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<City>());

        var handler = new GetCitiesByGovernorateQueryHandler(lookupRepositoryMock.Object, CreateMapper());

        var result = await handler.Handle(new GetCitiesByGovernorateQuery(999), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
