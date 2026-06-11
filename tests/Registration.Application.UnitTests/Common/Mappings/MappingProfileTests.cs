using AutoMapper;
using FluentAssertions;
using Registration.Application.Common.Mappings;
using Xunit;

namespace Registration.Application.UnitTests.Common.Mappings;

public class MappingProfileTests
{
    [Fact]
    public void MappingProfile_Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

        configuration.AssertConfigurationIsValid();
    }
}
