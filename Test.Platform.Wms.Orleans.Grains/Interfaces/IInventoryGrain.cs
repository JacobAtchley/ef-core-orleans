using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Orleans.Grains.Interfaces
{
    public interface IInventoryGrain : IGrainWithGuidKey
    {
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
        
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
    }
}