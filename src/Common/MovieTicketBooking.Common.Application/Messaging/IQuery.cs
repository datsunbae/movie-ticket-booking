using MovieTicketBooking.Common.Domain;
using MediatR;

namespace MovieTicketBooking.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
