using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Orleans.Grains.Implementations
{
    [StorageProvider(ProviderName = "inventoryStorage")]
    public class InventoryPersistenceGrain : Grain<Inventory>, IInventoryPersistenceGrain
    {
        private readonly IItemRepository _itemRepo;

        public InventoryPersistenceGrain(
            IItemRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            State.Count -= quantity;
            await WriteStateAsync();
            return State;
        }

        public async Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            if(State != null && State.Id != default)
            {
                State.Count += quantity;
            }
            else
            {
                var item = await _itemRepo.GetByKeyAsync(itemId, cancellationToken);
                
                if(item == null)
                {
                    throw new Exception($"Could not find item with given key: {itemId}");
                }
                
                State = new Inventory
                {
                    Id = Guid.NewGuid(),
                    Count = quantity,
                    Item = item, 
                    ItemId = item.Id
                };
            }

            await WriteStateAsync();
            
            return State;
        }
    }
}