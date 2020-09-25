using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;
using Test.Platform.Wms.Orleans.Grains.Interfaces;
using Test.Platform.Wms.Services.Interfaces;

namespace Test.Platform.Wms.Services.Implementations
{
    public class IncrementInventoryService : IIncrementInventoryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryGrain _grain;

        public IncrementInventoryService(IItemRepository itemRepository, IInventoryGrain grain)
        {
            _itemRepository = itemRepository;
            _grain = grain;
        }

        public Task<Inventory> IncrementAsync(Guid itemId, decimal amount, CancellationToken cancellationToken)
        {
            var item = _itemRepository.GetByKeyAsync(itemId, cancellationToken);

            if (item == null)
            {
                throw new Exception("Could not find item");
            }
            
            var inventory = _grain.
        }
    }
}