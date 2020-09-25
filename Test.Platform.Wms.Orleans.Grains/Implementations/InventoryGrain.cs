using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Orleans.Grains.Implementations
{
    public class InventoryGrain : Grain, IInventoryGrain
    {
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryGrain(IItemRepository itemRepository, IInventoryRepository inventoryRepository)
        {
            _itemRepository = itemRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByKeyAsync(itemId, cancellationToken);

            if (item == null)
            {
                throw new Exception("Could not find item");
            }

            var inventories = await _inventoryRepository
                .GetByItemId(itemId, cancellationToken);

            var inventory = inventories?.FirstOrDefault();

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    Count = quantity,
                    Id = Guid.NewGuid(),
                    ItemId = itemId
                };
                
                await _inventoryRepository.CreateAsync(inventory, cancellationToken);
            }
            else
            {
                inventory.Count += quantity;
                await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
            }

            return inventory;
        }

        public async Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByKeyAsync(itemId, cancellationToken);

            if (item == null)
            {
                throw new Exception("Could not find item");
            }

            var inventories = await _inventoryRepository
                .GetByItemId(itemId, cancellationToken);

            var inventory = inventories?.FirstOrDefault();

            if (inventory == null)
            {
                throw new Exception("Could not decrement inventory that doesn't exist.");
            }

            var count = inventory.Count -= quantity;

            if (count < 0)
            {
                throw new Exception("Could not decrement inventory. you would be negative.");
            }
                
            await _inventoryRepository.UpdateAsync(inventory, cancellationToken);

            return inventory;
        }
    }
}