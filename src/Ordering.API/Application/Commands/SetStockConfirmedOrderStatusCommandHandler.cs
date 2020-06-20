using CSharpFunctionalExtensions;
using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    // Regular CommandHandler
    public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;

        public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// Stock service confirms the request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<Result> Handle(SetStockConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
        {
            // Simulate a work time for confirming the stock
            await Task.Delay(10000, cancellationToken);

            var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
            if (orderToUpdate == null)
            {
                return Result.Failure("Order is null.");
            }

            orderToUpdate.SetStockConfirmedStatus();
             await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Result.Success();
        }
    }


}
