using System.Security.Claims;
using MovieTicketBooking.Common.Infrastructure.Authentication;
using MovieTicketBooking.Modules.Users.Application.Users.GetUser;

namespace MovieTicketBooking.Modules.Users.Presentation.Users;

internal sealed class GetUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/{id}", async (Ulid id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithOpenApi()
            .WithName(nameof(GetUserProfile))
            .WithTags(Tags.Users);
    }
}