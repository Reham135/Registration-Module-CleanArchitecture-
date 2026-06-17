using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Registration.Application.Common.Interfaces;
using Registration.Infrastructure.Email;
using Registration.Infrastructure.Messaging;
using Registration.Infrastructure.Messaging.Consumers;
using Registration.Infrastructure.Services;
using Registration.Persistence;

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

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<WelcomeEmailConsumer>();
            busConfigurator.AddConsumer<RegistrationAuditConsumer>();

            // MassTransit built-in Outbox — intercepts IPublishEndpoint calls inside a
            // handler and stores them in the MassTransit outbox tables within the same
            // EF Core transaction, then delivers them once the transaction commits.
            busConfigurator.AddEntityFrameworkOutbox<ApplicationDbContext>(outboxCfg =>
            {
                outboxCfg.UseSqlServer();

                // Poll for unsent messages every 5 seconds.
                outboxCfg.QueryDelay = TimeSpan.FromSeconds(5);

                // Suppress duplicate deliveries within a 30-second window.
                outboxCfg.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);

                // Enable the bus-side outbox so that IPublishEndpoint.Publish() calls
                // from non-consumer code (e.g. command handlers) are intercepted and
                // stored in the OutboxMessage table instead of going to RabbitMQ directly.
                outboxCfg.UseBusOutbox();
            });

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.UseMessageRetry(r => r.Intervals(
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(15),
                    TimeSpan.FromSeconds(30)));

                cfg.ConfigureEndpoints(context); // auto-creates one queue per consumer
            });
        });

        return services;
    }
}
