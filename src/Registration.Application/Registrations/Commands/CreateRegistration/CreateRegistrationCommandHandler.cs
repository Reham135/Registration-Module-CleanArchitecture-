using MassTransit;
using MediatR;
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
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateRegistrationCommandHandler(
        IRegistrationRepository registrationRepository,
        IDateTimeProvider dateTimeProvider,
        IPublishEndpoint publishEndpoint)
    {
        _registrationRepository = registrationRepository;
        _dateTimeProvider = dateTimeProvider;
        _publishEndpoint = publishEndpoint;
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
        await _registrationRepository.SaveChangesAsync(cancellationToken); // generates registration.Id

        var integrationEvent = new RegistrationCreatedIntegrationEvent(
            registration.Id,
            registration.FirstName.Value,
            registration.LastName.Value,
            registration.Email.Value,
            registration.MobileNumber.Value,
            _dateTimeProvider.Today.ToDateTime(TimeOnly.MinValue));

        // MassTransit intercepts this call and adds an OutboxMessage row to the DbContext
        // change tracker instead of sending to RabbitMQ immediately.
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        // Persist the OutboxMessage row that MassTransit added to the change tracker above.
        // The background OutboxDeliveryService then picks it up and forwards it to RabbitMQ.
        await _registrationRepository.SaveChangesAsync(cancellationToken);

        return registration.Id;
    }
}
