using CSharpFunctionalExtensions;
using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{

    // Regular CommandHandler
    public class SetAwaitingValidationOrderStatusCommandHandler : IRequestHandler<SetAwaitingValidationOrderStatusCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;

        public SetAwaitingValidationOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler which processes the command when
        /// graceperiod has finished
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<Result> Handle(SetAwaitingValidationOrderStatusCommand command, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
            if (orderToUpdate == null)
            {
                return Result.Failure("Order is null.");
            }

            orderToUpdate.SetAwaitingValidationStatus();
             await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
