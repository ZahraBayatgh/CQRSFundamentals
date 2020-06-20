using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System.Collections.Generic;

namespace Ordering.Domain.Events
{
    public class OrderStatusChangedToPaidDomainEvent
    : INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToPaidDomainEvent(int orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }

}
