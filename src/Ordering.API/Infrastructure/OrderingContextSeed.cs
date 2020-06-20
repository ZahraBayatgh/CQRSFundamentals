using Ordering.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class OrderingContextSeed
    {

        public OrderingContextSeed(OrderingContext context)
        {
            _context = context;
        }

        public readonly OrderingContext _context;

        public async Task SeedAsync()
        {
            using (_context)
            {
                try
                {
                    _context.Database.EnsureCreated();

                    if (!_context.OrderStatus.Any())
                    {
                        var orderStatus = new List<OrderStatus>()
                      {
                          OrderStatus.Submitted,
                          OrderStatus.AwaitingValidation,
                          OrderStatus.StockConfirmed,
                          OrderStatus.Paid,
                          OrderStatus.Shipped,
                          OrderStatus.Cancelled
                      };
                        _context.OrderStatus.AddRange(orderStatus);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (System.Exception)
                {

                }
            }
        }

    }
}
