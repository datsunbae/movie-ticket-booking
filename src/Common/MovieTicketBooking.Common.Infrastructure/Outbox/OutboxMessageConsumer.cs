namespace MovieTicketBooking.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumer(Ulid outboxMessageId, string name)
{
    public Ulid OutboxMessageId { get; init; } = outboxMessageId;
    public string Name { get; init; } = name;
}