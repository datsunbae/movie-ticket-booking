using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Common.Infrastructure.Database;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Data;
using MovieTicketBooking.Modules.Users.Domain.Users;

namespace MovieTicketBooking.Modules.Users.Infrastructure.Database;

public class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : MovieTicketBookingDbContext<UsersDbContext>(options), IUnitOfWork
{
    internal DbSet<User> Users => Set<User>();

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return this.Database.RollbackTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfigurationsFromAssembly(Common.Infrastructure.AssemblyReference.Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

}
