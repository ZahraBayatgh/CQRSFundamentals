using Ordering.API.Application.Models;
using System;
using System.Collections.Generic;

namespace Ordering.API.Application.DTOs
{
    public class OrderDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Buyer { get; set; }
        public Guid RequestId { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
    }

}
