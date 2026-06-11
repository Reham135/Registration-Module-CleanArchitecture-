using FluentAssertions;
using Registration.Domain.Entities;
using Registration.Domain.Exceptions;
using Xunit;

namespace Registration.Domain.UnitTests.Entities;

public class RegistrationEntityTests
{
    private static readonly DateOnly Today = new(2026, 6, 10);

    private static Address ValidAddress(bool isPrimary = true) =>
        Address.Create(governorateId: 1, cityId: 101, street: "10 Tahrir Street", buildingNumber: "12A", flatNumber: "3", isPrimary);

    private static Domain.Entities.Registration CreateRegistration(DateOnly birthDate, IReadOnlyCollection<Address> addresses) =>
        Domain.Entities.Registration.Create(
            firstName: "Ahmed",
            middleName: "Mohamed",
            lastName: "Ali",
            birthDate: birthDate,
            mobileNumber: "+201006158123",
            email: "ahmed.ali@example.com",
            addresses: addresses,
            today: Today);

    [Fact]
    public void Create_WithAgeExactly20_Succeeds()
    {
        var birthDate = Today.AddYears(-20);

        var act = () => CreateRegistration(birthDate, new[] { ValidAddress() });

        act.Should().NotThrow();
    }

    [Fact]
    public void Create_WithAgeOneDayShortOf20_Throws()
    {
        var birthDate = Today.AddYears(-20).AddDays(1);

        var act = () => CreateRegistration(birthDate, new[] { ValidAddress() });

        act.Should().Throw<DomainException>().WithMessage("*at least 20 years old*");
    }

    [Fact]
    public void Create_WithFutureBirthDate_Throws()
    {
        var birthDate = Today.AddDays(1);

        var act = () => CreateRegistration(birthDate, new[] { ValidAddress() });

        act.Should().Throw<DomainException>().WithMessage("*cannot be in the future*");
    }

    [Fact]
    public void Create_WithZeroAddresses_Throws()
    {
        var birthDate = Today.AddYears(-25);

        var act = () => CreateRegistration(birthDate, Array.Empty<Address>());

        act.Should().Throw<InvalidAddressConfigurationException>().WithMessage("*between 1 and 5 addresses*");
    }

    [Fact]
    public void Create_WithSixAddresses_Throws()
    {
        var birthDate = Today.AddYears(-25);
        var addresses = Enumerable.Range(0, 6).Select(i => ValidAddress(isPrimary: i == 0)).ToArray();

        var act = () => CreateRegistration(birthDate, addresses);

        act.Should().Throw<InvalidAddressConfigurationException>().WithMessage("*between 1 and 5 addresses*");
    }

    [Fact]
    public void Create_WithSingleAddress_AutoPromotesToPrimary()
    {
        var birthDate = Today.AddYears(-25);
        var address = ValidAddress(isPrimary: false);

        var registration = CreateRegistration(birthDate, new[] { address });

        registration.Addresses.Single().IsPrimary.Should().BeTrue();
    }

    [Fact]
    public void Create_WithMultipleAddressesAndExactlyOnePrimary_Succeeds()
    {
        var birthDate = Today.AddYears(-25);
        var addresses = new[] { ValidAddress(isPrimary: true), ValidAddress(isPrimary: false) };

        var registration = CreateRegistration(birthDate, addresses);

        registration.Addresses.Count(a => a.IsPrimary).Should().Be(1);
    }

    [Fact]
    public void Create_WithMultipleAddressesAndNoPrimary_Throws()
    {
        var birthDate = Today.AddYears(-25);
        var addresses = new[] { ValidAddress(isPrimary: false), ValidAddress(isPrimary: false) };

        var act = () => CreateRegistration(birthDate, addresses);

        act.Should().Throw<InvalidAddressConfigurationException>().WithMessage("*exactly one address*");
    }

    [Fact]
    public void Create_WithMultipleAddressesAndMultiplePrimary_Throws()
    {
        var birthDate = Today.AddYears(-25);
        var addresses = new[] { ValidAddress(isPrimary: true), ValidAddress(isPrimary: true) };

        var act = () => CreateRegistration(birthDate, addresses);

        act.Should().Throw<InvalidAddressConfigurationException>().WithMessage("*exactly one address*");
    }
}
