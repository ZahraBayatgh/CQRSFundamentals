using CSharpFunctionalExtensions;
using MediatR;

namespace Ordering.API.Application.Commands
{
    public class SetPaidOrderStatusCommand : IRequest<Result>
    {

        public int OrderNumber { get; private set; }

        public SetPaidOrderStatusCommand(int orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }

}
