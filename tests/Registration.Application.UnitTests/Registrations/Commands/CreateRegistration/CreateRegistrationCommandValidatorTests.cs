using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Registration.Application.Common.Interfaces;
using Registration.Application.Registrations.Commands.CreateRegistration;
using Registration.Application.Registrations.DTOs;
using Xunit;

namespace Registration.Application.UnitTests.Registrations.Commands.CreateRegistration;

public class CreateRegistrationCommandValidatorTests
{
    private static readonly DateOnly Today = new(2026, 6, 10);

    private readonly Mock<ILookupRepository> _lookupRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly CreateRegistrationCommandValidator _validator;

    public CreateRegistrationCommandValidatorTests()
    {
        _lookupRepositoryMock = new Mock<ILookupRepository>();
        _lookupRepositoryMock.Setup(r => r.GovernorateExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _lookupRepositoryMock.Setup(r => r.CityBelongsToGovernorateAsync(101, 1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(d => d.Today).Returns(Today);

        _validator = new CreateRegistrationCommandValidator(_lookupRepositoryMock.Object, _dateTimeProviderMock.Object);
    }

    private static CreateRegistrationCommand ValidCommand() => new()
    {
        FirstName = "Ahmed",
        MiddleName = "Mohamed",
        LastName = "Ali",
        BirthDate = Today.AddYears(-25),
        MobileNumber = "+201006158123",
        Email = "ahmed.ali@example.com",
        Addresses = new List<CreateAddressDto>
        {
            new() { GovernorateId = 1, CityId = 101, Street = "10 Tahrir Street", BuildingNumber = "12A", FlatNumber = "3", IsPrimary = true },
        },
    };

    [Fact]
    public async Task Validate_WithValidCommand_HasNoErrors()
    {
        var result = await _validator.TestValidateAsync(ValidCommand());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Ahmed123")]
    public async Task Validate_FirstName_Invalid(string? firstName)
    {
        var command = ValidCommand() with { FirstName = firstName! };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public async Task Validate_FirstName_TooLong()
    {
        var command = ValidCommand() with { FirstName = new string('a', 51) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public async Task Validate_MiddleName_Empty_IsAllowed()
    {
        var command = ValidCommand() with { MiddleName = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
    }

    [Fact]
    public async Task Validate_BirthDate_InFuture_HasError()
    {
        var command = ValidCommand() with { BirthDate = Today.AddDays(1) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Fact]
    public async Task Validate_BirthDate_AgeOneDayShortOf20_HasError()
    {
        var command = ValidCommand() with { BirthDate = Today.AddYears(-20).AddDays(1) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Fact]
    public async Task Validate_BirthDate_AgeExactly20_HasNoError()
    {
        var command = ValidCommand() with { BirthDate = Today.AddYears(-20) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    [Theory]
    [InlineData("01006158123")]
    [InlineData("not-a-number")]
    [InlineData("")]
    public async Task Validate_MobileNumber_Invalid(string mobileNumber)
    {
        var command = ValidCommand() with { MobileNumber = mobileNumber };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.MobileNumber);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("")]
    public async Task Validate_Email_Invalid(string email)
    {
        var command = ValidCommand() with { Email = email };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public async Task Validate_Addresses_Empty_HasError()
    {
        var command = ValidCommand() with { Addresses = new List<CreateAddressDto>() };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_Addresses_SixAddresses_HasError()
    {
        var address = new CreateAddressDto { GovernorateId = 1, CityId = 101, Street = "Street", BuildingNumber = "1", FlatNumber = "1", IsPrimary = false };
        var command = ValidCommand() with { Addresses = Enumerable.Range(0, 6).Select(_ => address).ToList() };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_Addresses_MultipleWithoutPrimary_HasError()
    {
        var address = new CreateAddressDto { GovernorateId = 1, CityId = 101, Street = "Street", BuildingNumber = "1", FlatNumber = "1", IsPrimary = false };
        var command = ValidCommand() with { Addresses = new List<CreateAddressDto> { address, address } };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_Addresses_MultipleWithMultiplePrimary_HasError()
    {
        var address = new CreateAddressDto { GovernorateId = 1, CityId = 101, Street = "Street", BuildingNumber = "1", FlatNumber = "1", IsPrimary = true };
        var command = ValidCommand() with { Addresses = new List<CreateAddressDto> { address, address } };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Addresses);
    }

    [Fact]
    public async Task Validate_Address_NonExistentGovernorate_HasError()
    {
        _lookupRepositoryMock.Setup(r => r.GovernorateExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = ValidCommand();
        command.Addresses[0].GovernorateId = 999;

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Addresses[0].GovernorateId");
    }

    [Fact]
    public async Task Validate_Address_CityNotInGovernorate_HasError()
    {
        _lookupRepositoryMock.Setup(r => r.CityBelongsToGovernorateAsync(999, 1, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var command = ValidCommand();
        command.Addresses[0].CityId = 999;

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Addresses[0].CityId");
    }

    [Theory]
    [InlineData("")]
    [InlineData("Building@1")]
    public async Task Validate_Address_BuildingNumber_Invalid(string buildingNumber)
    {
        var command = ValidCommand();
        command.Addresses[0].BuildingNumber = buildingNumber;

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Addresses[0].BuildingNumber");
    }

    [Fact]
    public async Task Validate_Address_Street_TooLong_HasError()
    {
        var command = ValidCommand();
        command.Addresses[0].Street = new string('a', 201);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor("Addresses[0].Street");
    }
}
