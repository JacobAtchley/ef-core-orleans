using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/inventory/grain/persistence/increment/{itemId}/{quantity}/{index}")]
    public class InventoryIncrementGrainPersistenceController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IGrainFactory _grainFactory;

        public InventoryIncrementGrainPersistenceController(IGrainFactory factory,
            ILogger<InventoryIncrementGrainPersistenceController> logger)
        {
            _grainFactory = factory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IncrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Got request {index}");

            var grain = _grainFactory.GetGrain<IInventoryPersistenceGrain>(itemId);
            var inventory = await grain.IncrementInventoryAsync(itemId, quantity, index, cancellationToken);

            return Ok(inventory);
        }
    }

    [ApiController]
    [Route("/api/v1/inventory/grain/persistence/decrement/{itemId}/{quantity}/{index}")]
    public class InventoryDecrementGrainPersistenceController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        private readonly ILogger _logger;

        public InventoryDecrementGrainPersistenceController(IGrainFactory factory,
            ILogger<InventoryDecrementGrainPersistenceController> logger)
        {
            _grainFactory = factory;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> DecrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Got request {index}");

            var grain = _grainFactory.GetGrain<IInventoryPersistenceGrain>(itemId);
            var inventory = await grain.DecrementInventoryAsync(itemId, quantity, index, cancellationToken);
            
            return Ok(inventory);
        }
    }
}