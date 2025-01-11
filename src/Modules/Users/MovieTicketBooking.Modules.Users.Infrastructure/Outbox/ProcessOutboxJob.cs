using System.Reflection;
using MovieTicketBooking.Common.Application.Clock;
using MovieTicketBooking.Common.Application.Data;
using MovieTicketBooking.Common.Infrastructure.Database;
using MovieTicketBooking.Common.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace MovieTicketBooking.Modules.Users.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger)
    : ProcessOutboxJobBase(dbConnectionFactory, serviceScopeFactory, dateTimeProvider, logger)
{
    protected override string ModuleName => "Users";
    protected override Assembly ApplicationAssembly => Application.AssemblyReference.Assembly;
    protected override string Schema => Schemas.Users;
    protected override int BatchSize => outboxOptions.Value.BatchSize;
}