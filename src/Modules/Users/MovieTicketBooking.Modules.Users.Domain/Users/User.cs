using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Modules.Users.Domain.Users;

public class User : Entity
{
    public Ulid Id { get; init; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string IdentityId { get; private set; } = string.Empty;

    private User() { }

    public static User Create(
        string email,
        string firstName,
        string lastName,
        string identityId)
    {
        var user = new User
        {
            Id = Ulid.NewUlid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityId = identityId
        };

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }
}
