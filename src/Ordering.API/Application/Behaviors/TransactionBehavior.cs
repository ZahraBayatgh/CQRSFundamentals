using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly OrderingContext _dbContext;

        public TransactionBehavior(OrderingContext dbContext,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(OrderingContext));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request.ToString().EndsWith("Query"))
            {
                return await next();
            }

            var response = default(TResponse);
            var typeName = request.GetType();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (_dbContext.HasActiveTransaction)
                    {
                        return await next();
                    }

                    var strategy = _dbContext.Database.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        Guid transactionId;

                        using (var transaction = await _dbContext.BeginTransactionAsync())
                        {
                            response = await next();
                            _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                            await _dbContext.CommitTransactionAsync(transaction);
                            transactionId = transaction.TransactionId;
                        }
                    });

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                    if (i >= 3)
                        throw;

                }
            }

            return response;
        }
    }
}
