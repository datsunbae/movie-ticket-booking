using MovieTicketBooking.Common.Domain;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Data;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Identity;
using MovieTicketBooking.Modules.Users.Domain.Users;

namespace MovieTicketBooking.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProvider,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityProvider.CreateUserAsync(
            new UserModel(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName), cancellationToken);
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);
        
        var user = User.Create(request.Email, request.FirstName, request.LastName, result.Value);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (UserResponse)user;
    }
}