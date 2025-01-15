using Microsoft.EntityFrameworkCore;

namespace MovieTicketBooking.Common.Infrastructure.Database;
public class MovieTicketBookingDbContext<T>(DbContextOptions<T> options) : DbContext(options) where T:DbContext
{

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        UlidToStringConverter.Convert(configurationBuilder);

        base.ConfigureConventions(configurationBuilder);
    }
}
