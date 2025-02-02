using Microsoft.Extensions.Logging;
using GameStore.Repositories;
using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class GameStoreService
    {
        private readonly GameRepository _gameRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ILogger<GameStoreService> _logger;

        public GameStoreService(GameRepository gameRepository, OrderRepository orderRepository, ILogger<GameStoreService> logger)
        {
            _gameRepository = gameRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<List<Game>> GetGamesWithDiscount(decimal percentage)
        {
            _logger.LogInformation("Fetching games with {Percentage}% discount", percentage);
            
            var games = await _gameRepository.GetAllAsync();
            foreach (var game in games)
            {
                game.Price -= (game.Price * percentage / 100);
            }

            _logger.LogInformation("{Count} games fetched with discount applied", games.Count);
            return games;
        }

        public async Task<Order> PlaceOrder(string gameId, string customerName)
        {
            _logger.LogInformation("Processing order for Game ID: {GameId} by {CustomerName}", gameId, customerName);

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                _logger.LogWarning("Game ID {GameId} not found", gameId);
                throw new Exception("Game not found");
            }

            var order = new Order
            {
                GameId = gameId,
                CustomerName = customerName,
                OrderDate = DateTime.UtcNow
            };

            await _orderRepository.CreateAsync(order);
            _logger.LogInformation("Order created successfully: {OrderId}", order.Id);

            return order;
        }
    }
}
