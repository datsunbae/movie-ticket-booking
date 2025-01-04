using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieTicketBooking.Common.Infrastructure.Outbox;

namespace MovieTicketBooking.Common.Infrastructure.Database;

public static class Postgres
{
    public static Action<IServiceProvider, DbContextOptionsBuilder> StandardOptions(IConfiguration configuration, string schema) =>
        (serviceProvider, options) =>
        {
            options.UseNpgsql(
                    configuration.GetConnectionString("Database")!,
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
                    }).UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    serviceProvider.GetRequiredService<InsertOutboxMessagesInterceptor>());
        };
}