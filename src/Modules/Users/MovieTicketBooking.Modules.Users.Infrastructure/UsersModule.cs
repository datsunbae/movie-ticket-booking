using MovieTicketBooking.Common.Application.Authorization;
using MovieTicketBooking.Common.Application.EventBus;
using MovieTicketBooking.Common.Application.Messaging;
using MovieTicketBooking.Common.Infrastructure.Database;
using MovieTicketBooking.Common.Presentation.Endpoints;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Data;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Identity;
using MovieTicketBooking.Modules.Users.Domain.Users;
using MovieTicketBooking.Modules.Users.Infrastructure.Database;
using MovieTicketBooking.Modules.Users.Infrastructure.Identity;
using MovieTicketBooking.Modules.Users.Infrastructure.Inbox;
using MovieTicketBooking.Modules.Users.Infrastructure.Outbox;
using MovieTicketBooking.Modules.Users.Infrastructure.Users;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MovieTicketBooking.Modules.Users.Application;


namespace MovieTicketBooking.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();

        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();

        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection
        AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            //.AddScoped<IPermissionService, PermissionService>()
            .AddIdentityProvider(configuration)
            .AddDatabase(configuration)
            .AddOutbox(configuration)
            .AddInbox(configuration);

    private static IServiceCollection AddIdentityProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"))
            .AddTransient<IIdentityProviderService, KeyCloakIdentityProviderService>();

        services.AddTransient<KeyCloakAuthDelegatingHandler>()
            .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<UsersDbContext>(Postgres.StandardOptions(configuration, Schemas.Users))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<UsersDbContext>())
            .AddScoped<IUserRepository, UserRepository>();

    private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"))
            .ConfigureOptions<ConfigureProcessOutboxJob>();

    private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<InboxOptions>(configuration.GetSection("Users:Inbox"))
            .ConfigureOptions<ConfigureProcessInboxJob>();

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                registrationConfigurator
                    .AddConsumer(typeof(IntegrationEventConsumer<>)
                    .MakeGenericType(integrationEventType))
                    .Endpoint(c => c.InstanceId = instanceId);
            });

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            var domainEventType = domainEventHandler
                .GetInterfaces()
                .Single(@interface => @interface.IsGenericType)
                .GetGenericArguments()
                .Single();

            var closedIdempotentHandlerType = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventType);

            services.Decorate(domainEventHandler, closedIdempotentHandlerType);
        }
    }

    private static void AddIntegrationEventHandlers(this IServiceCollection services) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                services.TryAddScoped(integrationEventHandlerType);

                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandlerType = typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEventType);

                services.Decorate(integrationEventHandlerType, closedIdempotentHandlerType);
            });
}