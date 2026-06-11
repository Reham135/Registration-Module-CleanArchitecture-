using FluentAssertions;
using Registration.Domain.Exceptions;
using Registration.Domain.ValueObjects;
using Xunit;

namespace Registration.Domain.UnitTests.ValueObjects;

public class PersonNameTests
{
    [Theory]
    [InlineData("Ahmed")]
    [InlineData("Al-Sayed")]
    [InlineData("O'Connor")]
    [InlineData("أحمد")]
    public void Create_WithValidValue_ReturnsPersonName(string value)
    {
        var name = PersonName.Create(value, "FirstName");

        name.Value.Should().Be(value);
    }

    [Fact]
    public void Create_TrimsAndCollapsesInternalWhitespace()
    {
        var name = PersonName.Create("  Ahmed   Mohamed  ", "FirstName");

        name.Value.Should().Be("Ahmed Mohamed");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_Throws(string? value)
    {
        var act = () => PersonName.Create(value, "FirstName");

        act.Should().Throw<DomainException>().WithMessage("FirstName is required.");
    }

    [Fact]
    public void Create_ExceedingMaxLength_Throws()
    {
        var value = new string('a', 51);

        var act = () => PersonName.Create(value, "FirstName");

        act.Should().Throw<DomainException>().WithMessage("*must not exceed 50 characters*");
    }

    [Theory]
    [InlineData("Ahmed123")]
    [InlineData("Ahmed@Mohamed")]
    [InlineData("Ahmed_Mohamed")]
    public void Create_WithInvalidCharacters_Throws(string value)
    {
        var act = () => PersonName.Create(value, "FirstName");

        act.Should().Throw<DomainException>().WithMessage("*invalid characters*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateOptional_WithEmptyValue_ReturnsNull(string? value)
    {
        var name = PersonName.CreateOptional(value, "MiddleName");

        name.Should().BeNull();
    }

    [Fact]
    public void CreateOptional_WithValue_ReturnsPersonName()
    {
        var name = PersonName.CreateOptional("Mohamed", "MiddleName");

        name.Should().NotBeNull();
        name!.Value.Should().Be("Mohamed");
    }

    [Fact]
    public void Equality_IsBasedOnValue()
    {
        var first = PersonName.Create("Ahmed", "FirstName");
        var second = PersonName.Create("Ahmed", "FirstName");

        first.Should().Be(second);
    }
}
