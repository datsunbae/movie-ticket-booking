using MovieTicketBooking.Modules.Users.Domain.Users;
using MovieTicketBooking.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MovieTicketBooking.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext db) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Users.ToListAsync(cancellationToken);
    }

    public Task<User?> GetDetailAsync(Ulid userId, CancellationToken cancellationToken = default)
    {
        return db.Users.SingleOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public void Insert(User user)
    {      
        db.Users.Add(user);
    }
}