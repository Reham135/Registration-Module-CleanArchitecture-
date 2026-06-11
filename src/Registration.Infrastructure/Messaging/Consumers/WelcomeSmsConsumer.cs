using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application.Common.IntegrationEvents;

namespace Registration.Infrastructure.Messaging.Consumers;

/// <summary>
/// Sends a welcome SMS to a newly registered person. Currently a logging stub -
/// swap the body for a real SMS provider call when one is available.
/// </summary>
public class WelcomeSmsConsumer : IConsumer<RegistrationCreatedIntegrationEvent>
{
    private readonly ILogger<WelcomeSmsConsumer> _logger;

    public WelcomeSmsConsumer(ILogger<WelcomeSmsConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RegistrationCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Would send welcome SMS to {MobileNumber}: 'Welcome {FirstName}, your registration #{RegistrationId} was received successfully.'",
            message.MobileNumber,
            message.FirstName,
            message.Id);

        return Task.CompletedTask;
    }
}
