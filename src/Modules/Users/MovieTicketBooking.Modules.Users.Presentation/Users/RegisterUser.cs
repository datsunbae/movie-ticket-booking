using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Modules.Users.Application.Users.RegisterUser;

namespace MovieTicketBooking.Modules.Users.Presentation.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/register", async (
                [FromBody] RegisterUserCommand request, 
                ISender sender, 
                HttpContext context, 
                LinkGenerator linkGenerator) =>
            {
                var result = await sender.Send(request);

                return result.Match(
                    () => Results.Created(
                        linkGenerator.GetUriByName(context, nameof(GetUserProfile), new { id = result.Value.Id }),
                        result.Value),
                    ApiResults.Problem);
            })
            .WithOpenApi()
            .AllowAnonymous()
            .WithName(nameof(RegisterUser))
            .WithTags(Tags.Users);
    }
}