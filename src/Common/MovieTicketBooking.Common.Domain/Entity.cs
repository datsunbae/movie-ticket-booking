﻿namespace MovieTicketBooking.Common.Domain;

public abstract class Entity
{
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