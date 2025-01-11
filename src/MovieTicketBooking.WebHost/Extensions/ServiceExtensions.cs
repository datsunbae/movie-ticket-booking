using MovieTicketBooking.WebHost.Middleware;
using MovieTicketBooking.WebHost.OpenTelemetry;
using MovieTicketBooking.Common.Application;
using MovieTicketBooking.Common.Infrastructure;
using MovieTicketBooking.Common.Infrastructure.EventBus;
using MovieTicketBooking.Modules.Users.Infrastructure;

namespace MovieTicketBooking.WebHost.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    internal static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
        });

        return services;
    }

    internal static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration,
        string databaseConnectionString,
        string cacheConnectionString)
    {
        var rabbitMqConnectionString = configuration.GetValue<string>("ConnectionStrings:Queue")!;

        services
            .AddApplication([
                Modules.Users.Application.AssemblyReference.Assembly,
            ])
            .AddInfrastructure(
                DiagnosticsConfig.ServiceName,
                [
                    UsersModule.ConfigureConsumers,
                ],
                rabbitMqConnectionString,
                databaseConnectionString,
                cacheConnectionString);

        services
            .AddUsersModule(configuration);

        return services;
    }
}