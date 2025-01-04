namespace MovieTicketBooking.Common.Application.EventBus;

public interface IIntegrationEvent
{
    Ulid Id { get; }
    DateTime OccurredAtUtc { get; }
}