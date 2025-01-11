using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Modules.Users.Infrastructure.Database;

namespace MovieTicketBooking.WebHost.Extensions;

public static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        ApplyMigrations<UsersDbContext>(scope);
    }

    private static void ApplyMigrations<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        
        context.Database.Migrate();
    }
}