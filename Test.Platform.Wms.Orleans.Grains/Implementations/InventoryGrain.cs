using System;
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
        private readonly IInventoryDecrementService _decrementService;
        private readonly IInventoryIncrementService _incrementService;

        public InventoryGrain(IInventoryDecrementService decrementService,
            IInventoryIncrementService incrementService)
        {
            _decrementService = decrementService;
            _incrementService = incrementService;
        }

        public Task<Inventory> IncrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            return _incrementService.IncrementInventoryAsync(itemId, quantity, index, cancellationToken);
        }

        public Task<Inventory> DecrementInventoryAsync(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            return _decrementService.DecrementInventoryAsync(itemId, quantity, index, cancellationToken);
        }
    }
}