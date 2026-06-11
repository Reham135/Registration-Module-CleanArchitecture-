using MediatR;
using Microsoft.Extensions.Logging;
using Registration.Application.Common.Exceptions;
using Registration.Application.Common.Interfaces;
using Registration.Application.Common.IntegrationEvents;
using Registration.Domain.Entities;
using Registration.Domain.ValueObjects;

namespace Registration.Application.Registrations.Commands.CreateRegistration;

public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, int>
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ILogger<CreateRegistrationCommandHandler> _logger;

    public CreateRegistrationCommandHandler(
        IRegistrationRepository registrationRepository,
        IDateTimeProvider dateTimeProvider,
        IIntegrationEventPublisher integrationEventPublisher,
        ILogger<CreateRegistrationCommandHandler> logger)
    {
        _registrationRepository = registrationRepository;
        _dateTimeProvider = dateTimeProvider;
        _integrationEventPublisher = integrationEventPublisher;
        _logger = logger;
    }

    public async Task<int> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = Email.Create(request.Email).NormalizedValue;
        var normalizedMobile = MobileNumber.Create(request.MobileNumber).Value;

        if (await _registrationRepository.ExistsByNormalizedEmailAsync(normalizedEmail, cancellationToken))
        {
            throw new ConflictException($"A registration with email '{request.Email}' already exists.");
        }

        if (await _registrationRepository.ExistsByMobileNumberAsync(normalizedMobile, cancellationToken))
        {
            throw new ConflictException($"A registration with mobile number '{request.MobileNumber}' already exists.");
        }

        var addresses = request.Addresses
            .Select(a => Address.Create(a.GovernorateId, a.CityId, a.Street, a.BuildingNumber, a.FlatNumber, a.IsPrimary))
            .ToList();

        var registration = Domain.Entities.Registration.Create(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.BirthDate,
            request.MobileNumber,
            request.Email,
            addresses,
            _dateTimeProvider.Today);

        await _registrationRepository.AddAsync(registration, cancellationToken);
        await _registrationRepository.SaveChangesAsync(cancellationToken);

        try
        {
            var integrationEvent = new RegistrationCreatedIntegrationEvent(
                registration.Id,
                registration.FirstName.Value,
                registration.LastName.Value,
                registration.Email.Value,
                registration.MobileNumber.Value,
                _dateTimeProvider.Today.ToDateTime(TimeOnly.MinValue));

            await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish RegistrationCreatedIntegrationEvent for registration {RegistrationId}.", registration.Id);
        }

        return registration.Id;
    }
}
