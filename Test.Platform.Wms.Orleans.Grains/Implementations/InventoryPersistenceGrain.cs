using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Orleans.Grains.Implementations
{
    public class InventoryPersistenceGrain : Grain, IInventoryPersistenceGrain
    {
        private readonly IItemRepository _itemRepo;
        private readonly IPersistentState<Inventory> _state;

        public InventoryPersistenceGrain(
            IItemRepository itemRepo,
            [PersistentState("inventory", "inventoryStorage")]
            IPersistentState<Inventory> state)
        {
            _itemRepo = itemRepo;
            _state = state;
        }

        public async Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            _state.State.Count -= quantity;
            await _state.WriteStateAsync();
            return _state.State;;
        }

        public async Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            if(_state.RecordExists && _state.State != null && _state.State.Id != default)
            {
                _state.State.Count += quantity;
            }
            else
            {
                var item = await _itemRepo.GetByKeyAsync(itemId, cancellationToken);
                
                if(item == null)
                {
                    throw new Exception($"Could not find item with given key: {itemId}");
                }
                
                _state.State = new Inventory
                {
                    Id = Guid.NewGuid(),
                    Count = quantity,
                    Item = item, 
                    ItemId = item.Id
                };
            }

            await _state.WriteStateAsync();
            
            return _state.State;
        }
    }
}