using MovieTicketBooking.Common.Application.Clock;

namespace MovieTicketBooking.Common.Infrastructure.Clock;

public class DateTimeProvider : IDateTimeProvider
{
    private DateTime _now;
    public DateTime UtcNow => _now;
    public DateTimeProvider()
    {
        _now = DateTime.UtcNow;
    }
}