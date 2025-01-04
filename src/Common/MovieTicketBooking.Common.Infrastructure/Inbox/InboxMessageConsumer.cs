namespace MovieTicketBooking.Common.Infrastructure.Inbox;

public sealed class InboxMessageConsumer(Ulid inboxMessageId, string name)
{
    public Ulid InboxMessageId { get; init; } = inboxMessageId;
    public string Name { get; init; } = name;
}