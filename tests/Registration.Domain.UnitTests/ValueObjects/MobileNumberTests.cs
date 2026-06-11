using FluentAssertions;
using Registration.Domain.Exceptions;
using Registration.Domain.ValueObjects;
using Xunit;

namespace Registration.Domain.UnitTests.ValueObjects;

public class MobileNumberTests
{
    [Theory]
    [InlineData("+201006158123")]
    [InlineData("+14155552671")]
    public void Create_WithValidE164Value_ReturnsMobileNumber(string value)
    {
        var mobile = MobileNumber.Create(value);

        mobile.Value.Should().Be(value);
    }

    [Theory]
    [InlineData("+20 100 615 8123", "+201006158123")]
    [InlineData("+20-100-615-8123", "+201006158123")]
    [InlineData("+20 (100) 6158123", "+201006158123")]
    public void Create_NormalizesFormatting(string input, string expected)
    {
        var mobile = MobileNumber.Create(input);

        mobile.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_Throws(string? value)
    {
        var act = () => MobileNumber.Create(value);

        act.Should().Throw<DomainException>().WithMessage("Mobile number is required.");
    }

    [Theory]
    [InlineData("01006158123")]      // missing leading +
    [InlineData("+0123456789")]      // leading zero after +
    [InlineData("+201")]             // too short
    [InlineData("not-a-number")]
    public void Create_WithInvalidFormat_Throws(string value)
    {
        var act = () => MobileNumber.Create(value);

        act.Should().Throw<DomainException>().WithMessage("*E.164*");
    }

    [Fact]
    public void Equality_IsBasedOnNormalizedValue()
    {
        var first = MobileNumber.Create("+20 100 615 8123");
        var second = MobileNumber.Create("+201006158123");

        first.Should().Be(second);
    }
}
