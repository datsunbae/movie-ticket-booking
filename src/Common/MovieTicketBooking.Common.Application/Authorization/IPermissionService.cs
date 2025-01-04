using MovieTicketBooking.Common.Domain;

namespace MovieTicketBooking.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId);
}