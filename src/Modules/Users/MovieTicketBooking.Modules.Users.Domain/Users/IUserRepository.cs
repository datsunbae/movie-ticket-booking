namespace MovieTicketBooking.Modules.Users.Domain.Users;
public interface IUserRepository
{
    Task<User?> GetDetailAsync(Ulid userId, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    void Insert(User user);
}