using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Common.Application.Exceptions;
public class AppException(string requestName,
    Error? error = default,
    Exception? innerException = default)
    : Exception("Application exception", innerException)
{
    public string RequestName { get; } = requestName;

    public Error? Error { get; } = error;
}