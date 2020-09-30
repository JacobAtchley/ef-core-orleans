using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Storage;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Orleans.Grains.Interfaces
{
    public interface IInventoryGrain : IGrainWithGuidKey
    {
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
        
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
    }

    public interface IInventoryPersistenceGrain : IGrainWithGuidKey
    {
        
        Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
        
        Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken);
    }
}