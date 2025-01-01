namespace MovieTicketBooking.Common.Domain;

public abstract class Entity
{
    public Entity(Ulid id)
    {
        Id = id;
    }
    public Ulid Id { get; init; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    private readonly List<IDomainEvent> _domainEvents = [];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}