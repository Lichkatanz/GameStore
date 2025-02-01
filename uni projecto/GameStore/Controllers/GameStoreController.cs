using FluentValidation;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models;
using System.Runtime.InteropServices;

namespace GameStore.Controllers
{
    [Route("api/gamestore")]
    [ApiController]
    public class GameStoreController : ControllerBase
    {
        private readonly GameStoreService _gameStoreService;
        private readonly IValidator<decimal> _discountValidator;
        private readonly IValidator<OrderRequest> _orderValidator;

        public GameStoreController(
            GameStoreService gameStoreService,
            IValidator<decimal> discountValidator,
            IValidator<OrderRequest> orderValidator)
        {
            _gameStoreService = gameStoreService;
            _discountValidator = discountValidator;
            _orderValidator = orderValidator;
        }

        // 1. GET: Get all games with discount (Validated)
        [HttpGet("discount/{percentage}")]
        public async Task<IActionResult> GetGamesWithDiscount(decimal percentage)
        {
            var validationResult = await _discountValidator.ValidateAsync(percentage);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var games = await _gameStoreService.GetGamesWithDiscount(percentage);
            return Ok(games);
        }

        // 2. POST: Place an order (Validated)
        [HttpPost("order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            var validationResult = await _orderValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var order = await _gameStoreService.PlaceOrder(request.GameId, request.CustomerName);
                return CreatedAtAction(nameof(PlaceOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    // DTO (Data Transfer Object) for order request
    public class OrderRequest
    {
        public string GameId { get; set; }
        public string CustomerName { get; set; }
    }
}
