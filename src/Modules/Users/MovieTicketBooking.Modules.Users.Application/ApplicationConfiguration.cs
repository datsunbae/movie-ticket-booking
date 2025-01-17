using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieTicketBooking.Modules.Users.Application.Behaviors;
using System.Reflection;

namespace MovieTicketBooking.Modules.Users.Application;
public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(ApplicationConfiguration).GetTypeInfo().Assembly;

        Common.Application.ApplicationConfiguration.AddApplication(services, assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembly);

            config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
        });

        return services;
    }
}