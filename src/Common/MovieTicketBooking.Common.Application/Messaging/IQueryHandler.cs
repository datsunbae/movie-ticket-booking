using MovieTicketBooking.Common.Domain;
using MediatR;

namespace MovieTicketBooking.Common.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;