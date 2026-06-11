namespace Registration.Application.Common.Interfaces;

/// <summary>
/// Provides the current date/time. Implemented in Infrastructure to keep
/// handlers and validators testable.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }

    DateOnly Today { get; }
}
