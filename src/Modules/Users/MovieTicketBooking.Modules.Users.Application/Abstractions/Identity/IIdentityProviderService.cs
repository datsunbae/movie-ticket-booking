using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> CreateUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateUserAsync(UserModel user, CancellationToken cancellationToken = default);
}