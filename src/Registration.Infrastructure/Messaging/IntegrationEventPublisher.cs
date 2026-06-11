using MassTransit;
using Registration.Application.Common.Interfaces;

namespace Registration.Infrastructure.Messaging;

/// <summary>
/// Publishes integration events to RabbitMQ via MassTransit.
/// </summary>
public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public IntegrationEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken) where TEvent : class
    {
        return _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
