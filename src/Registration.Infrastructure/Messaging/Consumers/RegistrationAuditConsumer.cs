using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application.Common.IntegrationEvents;

namespace Registration.Infrastructure.Messaging.Consumers;

/// <summary>
/// Records an audit trail entry for a newly created registration. Currently a logging stub -
/// swap the body for a write to an audit store/service when one is available.
/// </summary>
public class RegistrationAuditConsumer : IConsumer<RegistrationCreatedIntegrationEvent>
{
    private readonly ILogger<RegistrationAuditConsumer> _logger;

    public RegistrationAuditConsumer(ILogger<RegistrationAuditConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegistrationCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "AUDIT: Registration {RegistrationId} created for {Email} at {CreatedAtUtc:O}.",
            message.Id,
            message.Email,
            message.CreatedAtUtc);

        return Task.CompletedTask;
    }
}
