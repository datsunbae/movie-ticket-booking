using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieTicketBooking.Common.Application.Caching;
using MovieTicketBooking.Common.Application.Clock;
using MovieTicketBooking.Common.Application.Data;
using MovieTicketBooking.Common.Application.EventBus;
using MovieTicketBooking.Common.Infrastructure.Authentication;
using MovieTicketBooking.Common.Infrastructure.Authorization;
using MovieTicketBooking.Common.Infrastructure.Caching;
using MovieTicketBooking.Common.Infrastructure.Clock;
using MovieTicketBooking.Common.Infrastructure.Data;
using MovieTicketBooking.Common.Infrastructure.Database;
using MovieTicketBooking.Common.Infrastructure.Outbox;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using StackExchange.Redis;

namespace MovieTicketBooking.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string serviceName,
        Action<IRegistrationConfigurator, string>[] moduleConfigureConsumers,
        string rabbitMqConnectionString,
        string databaseConnectionString,
        string cacheConnectionString)
    {
        services.AddAuthenticationInternal();

        services.AddAuthorizationInternal();

        var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        Dapper.SqlMapper.AddTypeHandler(new StringUlidHandler());
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddQuartz(configurator =>
        {
            var scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {
            // HACK: Allows application to run without a Redis server. This is useful when, for example, generating a database migration.
            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit(configurator =>
        {
            string instanceId = serviceName.ToLowerInvariant().Replace('.', '-');
            foreach (Action<IRegistrationConfigurator, string> configureConsumer in moduleConfigureConsumers)
            {
                configureConsumer(configurator, instanceId);
            }

            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((context, config) =>
            {
                config.Host(rabbitMqConnectionString);
                config.ConfigureEndpoints(context);
            });
        });

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddNpgsql()
                    .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

                tracing.AddOtlpExporter();
            });

        return services;
    }
}