namespace MovieTicketBooking.Modules.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand

     : ICommand<UserResponse>
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; init; } = null!;
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; init; } = null!;
    /// <summary>
    /// FirstName
    /// </summary>
    public string FirstName { get; init; } = null!;
    /// <summary>
    /// LastName
    /// </summary>
    public string LastName { get; init; } = null!;
}