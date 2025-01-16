using MediatR;
using Microsoft.Extensions.Logging;
using MovieTicketBooking.Common.Application.Exceptions;
using MovieTicketBooking.Modules.Users.Application.Abstractions.Data;

namespace MovieTicketBooking.Modules.Users.Application.Behaviors;
public sealed class TransactionalPipelineBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (IsNotCommand())
            return await next();

        logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            TResponse response = await next();

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Commited transaction for {RequestName}", typeof(TRequest).Name);

            return response;
        }
        catch (Exception exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            logger.LogError("Rolled back transaction for {RequestName}", typeof(TRequest).Name);

            throw new AppException(typeof(TRequest).Name, innerException: exception);
        }
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}
