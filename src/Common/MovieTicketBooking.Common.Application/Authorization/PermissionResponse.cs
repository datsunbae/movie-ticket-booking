namespace MovieTicketBooking.Common.Application.Authorization;

public sealed record PermissionResponse(Ulid UserId, HashSet<string> Permissions);