using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Modules.Users.Domain.Users;

public sealed class UserRegisteredDomainEvent(Ulid userId) : DomainEvent
{
    public Ulid UserId { get; } = userId;
}