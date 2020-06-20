using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.Domain.Events
{
    public class OrderStatusChangedToAwaitingValidationDomainEvent
       : INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }

}
