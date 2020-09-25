using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Platform.Wms.Core.Interfaces;
using Test.Platform.Wms.Core.Models;

namespace Test.Platform.Wms.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _repo;

        public ItemController(IItemRepository context)
        {
            _repo = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllAsync(cancellationToken);
            
            return Ok(items);
        }

        [HttpGet("{id}", Name="GetItemById")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByKeyAsync(id, cancellationToken);

            if(item == null)
            {
                return NoContent();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repo.CreateAsync(item, cancellationToken);

            return CreatedAtRoute("GetItemById", new { item.Id }, item);
        }
    }
}