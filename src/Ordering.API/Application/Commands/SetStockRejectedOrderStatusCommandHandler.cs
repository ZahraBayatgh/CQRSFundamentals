using CSharpFunctionalExtensions;
using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    // Regular CommandHandler
    public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;

        public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// Stock service rejects the request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<Result> Handle(SetStockRejectedOrderStatusCommand command, CancellationToken cancellationToken)
        {
            // Simulate a work time for rejecting the stock
            await Task.Delay(10000, cancellationToken);

            var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
            if (orderToUpdate == null)
            {
                return Result.Failure("Order is null.");
            }

            orderToUpdate.SetCancelledStatusWhenStockIsRejected(command.OrderStockItems);

             await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Result.Success();
        }
    }

}
