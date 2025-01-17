using MovieTicketBooking.WebHost.Middleware;
using MovieTicketBooking.WebHost.OpenTelemetry;
using MovieTicketBooking.Common.Application;
using MovieTicketBooking.Common.Infrastructure;
using MovieTicketBooking.Common.Infrastructure.EventBus;
using MovieTicketBooking.Modules.Users.Infrastructure;
using Microsoft.OpenApi.Models;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

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

            options.SwaggerDoc("app", new OpenApiInfo { Title = "Movie Ticket Booking", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }});
            //var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            //options.IncludeXmlComments(xmlPath);

            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory()));
            foreach (var fi in dir.EnumerateFiles("*.xml"))
            {
                options.IncludeXmlComments(fi.FullName);
            }
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