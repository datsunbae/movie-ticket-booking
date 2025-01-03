using MovieTicketBooking.Common.Domain;
using MediatR;

namespace MovieTicketBooking.Common.Application.Messaging;

public interface IBaseCommand;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;
