namespace MovieTicketBooking.Common.Application.EventBus;

public abstract class IntegrationEvent(Ulid id, DateTime occurredAtUtc) : IIntegrationEvent
{
    public Ulid Id { get; init; } = id;
    public DateTime OccurredAtUtc { get; init; } = occurredAtUtc;
}