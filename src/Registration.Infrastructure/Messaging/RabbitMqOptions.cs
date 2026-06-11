namespace Registration.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ connection settings, bound from the "RabbitMq" configuration section.
/// </summary>
public class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; set; } = "localhost";

    public string VirtualHost { get; set; } = "/";

    public string Username { get; set; } = "guest";

    public string Password { get; set; } = "guest";
}
