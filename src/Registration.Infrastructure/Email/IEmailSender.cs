namespace Registration.Infrastructure.Email;

/// <summary>
/// Sends transactional emails (e.g. welcome emails) via SMTP.
/// </summary>
public interface IEmailSender
{
    Task SendAsync(string toAddress, string subject, string body, CancellationToken cancellationToken);
}
