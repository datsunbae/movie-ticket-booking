using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MovieTicketBooking.Common.Application.Behaviors;
using System.Reflection;

namespace MovieTicketBooking.Common.Application;
public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);

            config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);       

        return services;
    }
}