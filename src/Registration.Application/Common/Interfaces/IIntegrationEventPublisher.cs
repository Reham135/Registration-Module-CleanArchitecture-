namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Publishes integration events to the message broker. Implemented in the Infrastructure
/// layer so the Application layer does not depend on a specific messaging technology.
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken) where TEvent : class;
}
