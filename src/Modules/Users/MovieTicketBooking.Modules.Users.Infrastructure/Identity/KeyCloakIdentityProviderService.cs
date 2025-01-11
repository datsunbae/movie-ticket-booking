using System.Net;
using MovieTicketBooking.Common.Domain;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace MovieTicketBooking.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakIdentityProviderService(
    KeyCloakClient keyCloakClient,
    ILogger<KeyCloakIdentityProviderService> logger)
    : IIdentityProviderService
{ 
    private const string PasswordCredentialType = "Password";
    public async Task<Result<string>> CreateUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [
                new CredentialRepresentation(PasswordCredentialType, user.Password, false)
            ]);

        try
        {
            var identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
    }

    public Task<Result<string>> UpdateUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}