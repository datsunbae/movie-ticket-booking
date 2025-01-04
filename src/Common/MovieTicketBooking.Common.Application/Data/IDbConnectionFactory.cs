using System.Data.Common;

namespace MovieTicketBooking.Common.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}