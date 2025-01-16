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

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembly);

            config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies([assembly], includeInternalTypes: true);

        return services;
    }
}