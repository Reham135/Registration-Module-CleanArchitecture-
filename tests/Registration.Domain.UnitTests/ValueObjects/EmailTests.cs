using FluentAssertions;
using Registration.Domain.Exceptions;
using Registration.Domain.ValueObjects;
using Xunit;

namespace Registration.Domain.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("ahmed.ali@example.com")]
    [InlineData("a@b.co")]
    [InlineData("first.last+tag@sub.example.com")]
    public void Create_WithValidValue_ReturnsEmail(string value)
    {
        var email = Email.Create(value);

        email.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_Throws(string? value)
    {
        var act = () => Email.Create(value);

        act.Should().Throw<DomainException>().WithMessage("Email is required.");
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing-domain@")]
    [InlineData("@missing-local.com")]
    [InlineData("no-at-symbol.com")]
    public void Create_WithInvalidFormat_Throws(string value)
    {
        var act = () => Email.Create(value);

        act.Should().Throw<DomainException>().WithMessage("Email format is invalid.");
    }

    [Fact]
    public void Create_ExceedingMaxLength_Throws()
    {
        var localPart = new string('a', 250);
        var value = $"{localPart}@b.co";

        var act = () => Email.Create(value);

        act.Should().Throw<DomainException>().WithMessage("*must not exceed 254 characters*");
    }

    [Fact]
    public void NormalizedValue_IsLowercased()
    {
        var email = Email.Create("Ahmed.Ali@Example.COM");

        email.Value.Should().Be("Ahmed.Ali@Example.COM");
        email.NormalizedValue.Should().Be("ahmed.ali@example.com");
    }

    [Fact]
    public void Equality_IsCaseInsensitive_BasedOnNormalizedValue()
    {
        var first = Email.Create("Ahmed.Ali@Example.com");
        var second = Email.Create("ahmed.ali@example.COM");

        first.Should().Be(second);
    }
}
