namespace Registration.Infrastructure.Messaging;

/// <summary>
/// Settings for the outbox background processor, bound from the "Outbox" configuration section.
/// </summary>
public class OutboxOptions
{
    public const string SectionName = "Outbox";

    public int PollingIntervalSeconds { get; set; } = 5;

    public int BatchSize { get; set; } = 20;
}
