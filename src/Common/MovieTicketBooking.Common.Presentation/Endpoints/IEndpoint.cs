using Microsoft.AspNetCore.Routing;

namespace MovieTicketBooking.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}