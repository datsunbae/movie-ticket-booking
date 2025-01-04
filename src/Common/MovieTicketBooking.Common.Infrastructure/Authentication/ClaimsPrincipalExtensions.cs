using System.Security.Claims;
using MovieTicketBooking.Common.Application.Exceptions;

namespace MovieTicketBooking.Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Ulid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(CustomClaims.Sub);

        return Ulid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new AppException("User identifier is unavailable");
    }
    
    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new AppException("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        var permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                               throw new AppException("Permissions are unavailable");
        return permissionClaims.Select(claim => claim.Value).ToHashSet();
    }
}