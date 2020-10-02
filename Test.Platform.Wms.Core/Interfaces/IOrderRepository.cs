using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Core.Interfaces
{
    public interface IOrderRepository : ICrudRepo<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithNumberOfLines(int numberOfLines, CancellationToken cancellationToken);

        Task<IEnumerable<OrderSummary>> GetOrderSummaries(CancellationToken cancellationToken);
    }
}