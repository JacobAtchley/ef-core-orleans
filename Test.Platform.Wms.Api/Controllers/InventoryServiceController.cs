using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Api.Controllers
{
    
    
    [ApiController]
    [Route("/api/v1/inventory/service/increment/{itemId}/{quantity}/{index}")]
    public class InventoryIncrementServiceController : ControllerBase
    {
        private readonly ILogger _logger;
        
        private readonly IInventoryIncrementService _incrementService;

        public InventoryIncrementServiceController(IInventoryIncrementService incrementService,
            ILogger<InventoryIncrementServiceController> logger)
        {
            _incrementService = incrementService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IncrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Got request {index}");

            var inventory = await _incrementService.IncrementInventoryAsync(itemId, quantity, index, cancellationToken);

            return Ok(inventory);
        }
    }

    [ApiController]
    [Route("/api/v1/inventory/service/decrement/{itemId}/{quantity}/{index}")]
    public class InventoryDecrementServiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IInventoryDecrementService _decrementService;

        public InventoryDecrementServiceController(IInventoryDecrementService decrementService,
            ILogger<InventoryDecrementServiceController> logger)
        {
            _decrementService = decrementService;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> DecrementInventory(Guid itemId, decimal quantity, int index, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Got request {index}"); 
            var inventory = await _decrementService.DecrementInventoryAsync(itemId, quantity, index, cancellationToken);

            return Ok(inventory);
        }
    }
}