using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Test.Platform.Wms.Orleans.Grains.Interfaces;

namespace Test.Platform.Wms.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public InventoryController(IGrainFactory factory)
        {
            _grainFactory = factory;
        }

        [HttpPost("increment/{itemId}/{quantity}")]
        public async Task<IActionResult> IncrementInventory(Guid itemId, decimal quantity, CancellationToken cancellationToken)
        {
            var grain = _grainFactory.GetGrain<IInventoryGrain>(itemId);
            var inventory = await grain.IncrementInventoryAsync(itemId, quantity, cancellationToken);
            return Ok(inventory);
        }

        [HttpPost("decrement/{itemId}/{quantity}")]
        public async Task<IActionResult> DecrementInventory(Guid itemId, decimal quantity, CancellationToken cancellationToken)
        {
            var grain = _grainFactory.GetGrain<IInventoryGrain>(itemId);
            var inventory = await grain.DecrementInventoryAsync(itemId, quantity, cancellationToken);
            return Ok(inventory);
        }
    }
}