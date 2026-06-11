using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application.Common.IntegrationEvents;

namespace Registration.Infrastructure.Messaging.Consumers;

/// <summary>
/// Notifies an external/downstream service about a newly created registration. Currently a
/// logging stub - swap the body for a real HTTP call/message to the downstream service when available.
/// </summary>
public class ExternalServiceNotificationConsumer : IConsumer<RegistrationCreatedIntegrationEvent>
{
    private readonly ILogger<ExternalServiceNotificationConsumer> _logger;

    public ExternalServiceNotificationConsumer(ILogger<ExternalServiceNotificationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegistrationCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Would notify external service about new registration {RegistrationId} ({FirstName} {LastName}).",
            message.Id,
            message.FirstName,
            message.LastName);

        return Task.CompletedTask;
    }
}
