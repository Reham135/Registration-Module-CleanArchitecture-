using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Registration.Application.Common.Interfaces;

namespace Registration.Infrastructure.Messaging;

/// <summary>
/// Periodically publishes unprocessed <see cref="Registration.Application.Common.Models.OutboxMessage"/>
/// rows to the message broker (Outbox Pattern), decoupling persistence from broker availability.
/// </summary>
public class OutboxProcessorBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly OutboxOptions _options;
    private readonly ILogger<OutboxProcessorBackgroundService> _logger;

    public OutboxProcessorBackgroundService(
        IServiceScopeFactory scopeFactory,
        IOptions<OutboxOptions> options,
        ILogger<OutboxProcessorBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delay = TimeSpan.FromSeconds(_options.PollingIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing outbox messages.");
            }

            await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var messages = await outboxRepository.GetUnprocessedMessagesAsync(_options.BatchSize, cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type)
                    ?? throw new InvalidOperationException($"Could not resolve type '{message.Type}'.");

                var payload = JsonSerializer.Deserialize(message.Content, type)
                    ?? throw new InvalidOperationException($"Could not deserialize outbox message {message.Id} as '{message.Type}'.");

                await publishEndpoint.Publish(payload, type, cancellationToken);

                await outboxRepository.MarkAsProcessedAsync(message.Id, DateTime.UtcNow, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish outbox message {OutboxMessageId}.", message.Id);

                await outboxRepository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }
}
