using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Cosmo.Contexts;
using Test.Platform.Wms.EntityFramework.Abstractions;

namespace Test.Platform.Wms.Cosmo.Implementations
{
    public class OrderRepository :AbstractDbContextCrudRepo<Order, OrderContext>,  IOrderRepository
    {
        private readonly OrderContext _orderContext;

        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<IEnumerable<Order>> GetOrdersWithNumberOfLines(int numberOfLines, CancellationToken cancellationToken)
        {

            //this doesn't work with ef cosmos ;(
            var list = await _orderContext
                .Orders
                .Where(x => x.Lines.Count() > numberOfLines)
                .ToListAsync(cancellationToken);

            return list;
        }

        public async Task<IEnumerable<OrderSummary>> GetOrderSummaries(CancellationToken cancellationToken)
        {
            var list = await _orderContext
                .Orders
                .SelectMany(x => x.Lines.Select(l => l.Item), (order, item) => new
                {
                    Order = order,
                    Item = item
                })
                .GroupBy(x => x)
                .Select(x => new OrderSummary
                    {
                        Id = x.Key.Order.Id,
                        ItemCounts = x.Select(c => new ItemCount
                        {
                            Count = x.Count(),
                            Item = c.Item
                        })
                    }
                )
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}