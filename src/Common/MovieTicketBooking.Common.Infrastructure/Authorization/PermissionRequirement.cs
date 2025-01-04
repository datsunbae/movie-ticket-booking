using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Common.Infrastructure.Authorization;

internal sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}