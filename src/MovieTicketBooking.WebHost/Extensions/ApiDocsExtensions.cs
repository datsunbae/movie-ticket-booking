namespace MovieTicketBooking.WebHost.Extensions;
using Scalar.AspNetCore;

internal static class ApiDocsExtensions
{
    internal static IApplicationBuilder UseApiDocs(this WebApplication app)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "/openapi/{documentName}.json";
        });

        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Movie Ticket Boobking")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
                .WithLayout(ScalarLayout.Modern);
            options.EndpointPathPrefix = "api-docs/{documentName}";
        });

        return app;
    }
}
