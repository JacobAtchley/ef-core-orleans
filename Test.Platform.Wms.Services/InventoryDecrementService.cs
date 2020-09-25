using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Services
{
    public class InventoryDecrementService : IInventoryDecrementService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;

        public InventoryDecrementService(IItemRepository itemRepository, IInventoryRepository inventoryRepository)
        {
            _itemRepository = itemRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Decrement inventory index {index} {DateTime.Now}");
            
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

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