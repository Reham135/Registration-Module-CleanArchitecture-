using Registration.Application.Common.Models;

namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Provides access to outbox messages for the background processor that publishes them
/// to the message broker.
/// </summary>
public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize, CancellationToken cancellationToken);

    Task MarkAsProcessedAsync(Guid id, DateTime processedOnUtc, CancellationToken cancellationToken);

    Task MarkAsFailedAsync(Guid id, string error, CancellationToken cancellationToken);
}
