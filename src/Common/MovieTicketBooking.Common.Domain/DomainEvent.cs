namespace MovieTicketBooking.Common.Domain;
public abstract class DomainEvent : IDomainEvent
{
    public Ulid Id { get; init; }
    public DateTime OccurredAtUtc { get; init; }

    protected DomainEvent()
    {
        Id = Ulid.NewUlid();
        OccurredAtUtc = DateTime.UtcNow;
    }

    protected DomainEvent(Ulid id, DateTime occurredAtUtc)
    {
        Id = id;
        OccurredAtUtc = occurredAtUtc;
    }
}