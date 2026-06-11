using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Registration.Infrastructure.Email;

/// <summary>
/// Sends emails via SMTP (configured for Gmail by default - smtp.gmail.com:587 with STARTTLS).
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly EmailOptions _options;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<EmailOptions> options, ILogger<SmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(string toAddress, string subject, string body, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.SmtpHost) || string.IsNullOrWhiteSpace(_options.Password))
        {
            _logger.LogWarning("Email sending is not configured (missing SmtpHost or Password). Skipping email to {ToAddress} with subject '{Subject}'.", toAddress, subject);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_options.FromAddress, _options.FromDisplayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };
        message.To.Add(toAddress);

        using var client = new SmtpClient(_options.SmtpHost, _options.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_options.Username, _options.Password),
        };

        await client.SendMailAsync(message, cancellationToken);

        _logger.LogInformation("Sent email to {ToAddress} with subject '{Subject}'.", toAddress, subject);
    }
}
