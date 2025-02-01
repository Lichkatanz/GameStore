using GameStore.Models;
using GameStore.Repositories;
using System.Runtime.InteropServices;

ususing GameStore.Models;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _orderRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            await _orderRepository.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Order order)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null) return NotFound();

            order.Id = id;
            await _orderRepository.UpdateAsync(id, order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null) return NotFound();

            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
