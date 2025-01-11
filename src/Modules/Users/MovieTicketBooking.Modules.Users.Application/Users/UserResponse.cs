using MovieTicketBooking.Modules.Users.Domain.Users;

namespace MovieTicketBooking.Modules.Users.Application.Users;

public record UserResponse(
    Ulid Id,
    string Email,
    string FirstName,
    string LastName)
{
    public static implicit operator UserResponse(User user)
    {
        return new(user.Id,
            user.Email,
            user.FirstName,
            user.LastName);
    }
        
}