namespace MovieTicketBooking.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Ulid UserId) : IQuery<UserResponse>;