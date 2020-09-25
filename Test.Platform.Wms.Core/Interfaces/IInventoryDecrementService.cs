using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Core.Interfaces
{
    public interface IInventoryDecrementService
    {
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
    }
}