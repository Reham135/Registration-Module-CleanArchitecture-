using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application.Common.IntegrationEvents;
using Registration.Infrastructure.Email;

namespace Registration.Infrastructure.Messaging.Consumers;

/// <summary>
/// Sends a welcome email to a newly registered person.
/// </summary>
public class WelcomeEmailConsumer : IConsumer<RegistrationCreatedIntegrationEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<WelcomeEmailConsumer> _logger;

    public WelcomeEmailConsumer(IEmailSender emailSender, ILogger<WelcomeEmailConsumer> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RegistrationCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        var fullName = $"{message.FirstName} {message.LastName}";

        var subject = "Welcome to the Registration Module!";
        var body =
            $"""
             Hi {fullName},

             Thank you for registering with us. Your registration (#{message.Id}) has been received successfully.

             Best regards,
             The Registration Team
             """;

        await _emailSender.SendAsync(message.Email, subject, body, context.CancellationToken);

        _logger.LogInformation("Welcome email processed for registration {RegistrationId} ({Email}).", message.Id, message.Email);
    }
}
