using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Platform.Wms.Core.Interfaces;

namespace Test.Platform.Wms.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/orders/")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(cancellationToken);
            
            return Ok(orders);
        }

        [HttpGet("with-number-lines/{lines:int}")]
        public async Task<IActionResult> GetMoreThan(
            [FromRoute] int lines,
            CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersWithNumberOfLines(lines, cancellationToken);

            return Ok(orders);
        }
    }
}