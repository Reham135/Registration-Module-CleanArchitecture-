namespace Registration.Infrastructure.Email;

/// <summary>
/// SMTP settings used to send transactional emails (e.g. welcome emails), bound from the "Email" configuration section.
/// </summary>
public class EmailOptions
{
    public const string SectionName = "Email";

    public string SmtpHost { get; set; } = string.Empty;

    public int Port { get; set; } = 587;

    public string FromAddress { get; set; } = string.Empty;

    public string FromDisplayName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
