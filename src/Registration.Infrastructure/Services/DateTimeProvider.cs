using Registration.Application.Common.Interfaces;

namespace Registration.Infrastructure.Services;

/// <summary>
/// Default implementation of <see cref="IDateTimeProvider"/> backed by <see cref="DateTime.UtcNow"/>.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
