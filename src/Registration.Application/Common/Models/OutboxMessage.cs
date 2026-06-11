using System.Text.Json;

namespace Registration.Application.Common.Models;

/// <summary>
/// Represents an integration event persisted alongside business data in the same transaction,
/// to be published to the message broker by a separate background processor (Outbox Pattern).
/// </summary>
public class OutboxMessage
{
    public Guid Id { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTime OccurredOnUtc { get; private set; }

    public DateTime? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// </summary>
    private OutboxMessage()
    {
    }

    public static OutboxMessage Create<TEvent>(TEvent integrationEvent, DateTime occurredOnUtc) where TEvent : class
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(TEvent).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            OccurredOnUtc = occurredOnUtc,
        };
    }

    public void MarkAsProcessed(DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
    }
}
