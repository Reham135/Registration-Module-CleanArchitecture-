using Microsoft.Extensions.DependencyInjection;
using Registration.Application.Common.Interfaces;
using Registration.Infrastructure.Services;

namespace Registration.Infrastructure;

/// <summary>
/// Composition root extensions for registering Infrastructure-layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
