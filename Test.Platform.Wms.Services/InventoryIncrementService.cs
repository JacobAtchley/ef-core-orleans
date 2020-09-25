using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Services
{
    public class InventoryIncrementService : IInventoryIncrementService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;

        public InventoryIncrementService(IItemRepository itemRepository, IInventoryRepository inventoryRepository)
        {
            _itemRepository = itemRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Increment inventory index {index}");

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
    }
}