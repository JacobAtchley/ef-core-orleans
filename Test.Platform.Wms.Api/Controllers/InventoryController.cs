using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/inventory/grain/increment/{itemId}/{quantity}/{index}")]
    public class InventoryIncrementGrainController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public InventoryIncrementGrainController(IGrainFactory factory)
        {
            _grainFactory = factory;
        }

        [HttpPost]
        public async Task<IActionResult> IncrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Got request {index}");
            var grain = _grainFactory.GetGrain<IInventoryGrain>(itemId);
            var inventory = await grain.IncrementInventoryAsync(itemId, quantity, index, cancellationToken);
            return Ok(inventory);
        }
    }

    [ApiController]
    [Route("/api/v1/inventory/grain/decrement/{itemId}/{quantity}/{index}")]
    public class InventoryDecrementGrainController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public InventoryDecrementGrainController(IGrainFactory factory)
        {
            _grainFactory = factory;
        }
        
        [HttpPost]
        public async Task<IActionResult> DecrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Got request {index}");
            var grain = _grainFactory.GetGrain<IInventoryGrain>(itemId);
            var inventory = await grain.DecrementInventoryAsync(itemId, quantity, index, cancellationToken);
            return Ok(inventory);
        }
    }
    
    [ApiController]
    [Route("/api/v1/inventory/service/increment/{itemId}/{quantity}/{index}")]
    public class InventoryIncrementServiceController : ControllerBase
    {
        private readonly IInventoryIncrementService _incrementService;

        public InventoryIncrementServiceController(IInventoryIncrementService incrementService)
        {
            _incrementService = incrementService;
        }

        [HttpPost]
        public async Task<IActionResult> IncrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Got request {index}");
            var inventory = await _incrementService.IncrementInventoryAsync(itemId, quantity, index, cancellationToken);
            return Ok(inventory);
        }
    }

    [ApiController]
    [Route("/api/v1/inventory/service/decrement/{itemId}/{quantity}/{index}")]
    public class InventoryDecrementServiceController : ControllerBase
    {
        private readonly IInventoryDecrementService _decrementService;

        public InventoryDecrementServiceController(IInventoryDecrementService decrementService)
        {
            _decrementService = decrementService;
        }
        
        [HttpPost]
        public async Task<IActionResult> DecrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Got request {index}");
            var inventory = await _decrementService.DecrementInventoryAsync(itemId, quantity, index, cancellationToken);
            return Ok(inventory);
        }
    }
}