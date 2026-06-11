using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Registration.Application.Common.Behaviours;

namespace Registration.Application;

/// <summary>
/// Composition root extensions for registering Application-layer services
/// (MediatR, FluentValidation, AutoMapper).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
