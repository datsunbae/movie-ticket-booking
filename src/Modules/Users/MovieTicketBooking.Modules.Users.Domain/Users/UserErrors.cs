using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Modules.Users.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Ulid userId) =>
        Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found.");
    
    public static Error NotFound(string identityId) =>
        Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found.");
}