using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Core.Interfaces
{
    public interface IInventoryIncrementService
    {
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
    }
}