using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MovieTicketBooking.Common.Domain;
using MovieTicketBooking.Common.Infrastructure.Serialization;
using Newtonsoft.Json;
using System.Data.Common;

namespace MovieTicketBooking.Common.Infrastructure.Outbox;

public sealed class InsertOutboxMessagesInterceptor : DbTransactionInterceptor
{
    public override async ValueTask<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction, TransactionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is not null)
        {
            InsertOutboxMessages(dbContext);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return await base.TransactionCommittingAsync(transaction, eventData, result, cancellationToken);
    }
   
    private static void InsertOutboxMessages(
        DbContext context)
    {
        var outboxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.Id,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance),
                OccurredAtUtc = domainEvent.OccurredAtUtc,
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}