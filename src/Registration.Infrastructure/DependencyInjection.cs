using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Registration.Application.Common.Interfaces;
using Registration.Infrastructure.Email;
using Registration.Infrastructure.Messaging;
using Registration.Infrastructure.Messaging.Consumers;
using Registration.Infrastructure.Services;

namespace Registration.Infrastructure;

/// <summary>
/// Composition root extensions for registering Infrastructure-layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.AddTransient<IEmailSender, SmtpEmailSender>();

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<WelcomeEmailConsumer>();
            busConfigurator.AddConsumer<WelcomeSmsConsumer>();
            busConfigurator.AddConsumer<RegistrationAuditConsumer>();
            busConfigurator.AddConsumer<ExternalServiceNotificationConsumer>();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
