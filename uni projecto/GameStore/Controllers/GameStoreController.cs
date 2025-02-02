using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameStore.Services;
using GameStore.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    [Route("api/gamestore")]
    [ApiController]
    public class GameStoreController : ControllerBase
    {
        private readonly GameStoreService _gameStoreService;
        private readonly ILogger<GameStoreController> _logger;
        private readonly IValidator<decimal> _discountValidator;
        private readonly IValidator<OrderRequest> _orderValidator;

        public GameStoreController(
            GameStoreService gameStoreService, 
            ILogger<GameStoreController> logger,
            IValidator<decimal> discountValidator,
            IValidator<OrderRequest> orderValidator)
        {
            _gameStoreService = gameStoreService;
            _logger = logger;
            _discountValidator = discountValidator;
            _orderValidator = orderValidator;
        }

        [HttpGet("discount/{percentage}")]
        public async Task<IActionResult> GetGamesWithDiscount(decimal percentage)
        {
            _logger.LogInformation("Received request to get discounted games with {Percentage}% discount", percentage);

            var validationResult = await _discountValidator.ValidateAsync(percentage);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid discount request: {Errors}", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var games = await _gameStoreService.GetGamesWithDiscount(percentage);
            return Ok(games);
        }

        [HttpPost("order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            _logger.LogInformation("Received order request for Game ID: {GameId} by {CustomerName}", request.GameId, request.CustomerName);

            var validationResult = await _orderValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid order request: {Errors}", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                var order = await _gameStoreService.PlaceOrder(request.GameId, request.CustomerName);
                return CreatedAtAction(nameof(PlaceOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing order for Game ID: {GameId}", request.GameId);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
