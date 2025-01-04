using System.Data.Common;
using MovieTicketBooking.Common.Application.Data;
using Npgsql;

namespace MovieTicketBooking.Common.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
