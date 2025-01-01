using MediatR;

namespace MovieTicketBooking.Common.Domain;
public interface IDomainEvent : INotification
{
    Ulid Id { get; }
    DateTime OccurredAtUtc { get; }
}