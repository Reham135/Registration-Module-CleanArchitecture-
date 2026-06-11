using FluentAssertions;
using Moq;
using Registration.Application.Common.Exceptions;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.Models;
using Registration.Application.Registrations.Commands.CreateRegistration;
using Registration.Application.Registrations.DTOs;
using Xunit;

namespace Registration.Application.UnitTests.Registrations.Commands.CreateRegistration;

public class CreateRegistrationCommandHandlerTests
{
    private static readonly DateOnly Today = new(2026, 6, 10);

    private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly CreateRegistrationCommandHandler _handler;

    public CreateRegistrationCommandHandlerTests()
    {
        _registrationRepositoryMock = new Mock<IRegistrationRepository>();
        _registrationRepositoryMock.Setup(r => r.ExistsByNormalizedEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _registrationRepositoryMock.Setup(r => r.ExistsByMobileNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _registrationRepositoryMock
            .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<Task> operation, CancellationToken _) => operation());

        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(d => d.Today).Returns(Today);

        _handler = new CreateRegistrationCommandHandler(
            _registrationRepositoryMock.Object,
            _dateTimeProviderMock.Object);
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
    public async Task Handle_WithValidCommand_PersistsRegistrationAndReturnsId()
    {
        var result = await _handler.Handle(ValidCommand(), CancellationToken.None);

        result.Should().Be(0); // Id is assigned by the database; not set in this in-memory scenario.

        _registrationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Registration>(), It.IsAny<CancellationToken>()), Times.Once);
        _registrationRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_WithValidCommand_AddsOutboxMessage()
    {
        await _handler.Handle(ValidCommand(), CancellationToken.None);

        _registrationRepositoryMock.Verify(
            r => r.AddOutboxMessageAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ThrowsConflictException()
    {
        _registrationRepositoryMock.Setup(r => r.ExistsByNormalizedEmailAsync("ahmed.ali@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = () => _handler.Handle(ValidCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*email*");

        _registrationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Registration>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithDuplicateMobileNumber_ThrowsConflictException()
    {
        _registrationRepositoryMock.Setup(r => r.ExistsByMobileNumberAsync("+201006158123", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = () => _handler.Handle(ValidCommand(), CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>().WithMessage("*mobile number*");

        _registrationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Registration>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
