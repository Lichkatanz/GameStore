using GameStore.Models;
using GameStore.Repositories;

namespace GameStore.Services
{
    public class GameStoreService
    {
        private readonly GameRepository _gameRepository;
        private readonly OrderRepository _orderRepository;

        public GameStoreService(GameRepository gameRepository, OrderRepository orderRepository)
        {
            _gameRepository = gameRepository;
            _orderRepository = orderRepository;
        }

        // 1. Get all games with a discount applied
        public async Task<List<Game>> GetGamesWithDiscount(decimal discountPercentage)
        {
            var games = await _gameRepository.GetAllAsync();

            foreach (var game in games)
            {
                game.Price -= game.Price * (discountPercentage / 100);
            }

            return games;
        }

        // 2. Place an order for a game
        public async Task<Order> PlaceOrder(string gameId, string customerName)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new Exception("Game not found.");
            }

            var newOrder = new Order
            {
                GameId = gameId,
                CustomerName = customerName,
                OrderDate = DateTime.UtcNow
            };

            await _orderRepository.CreateAsync(newOrder);
            return newOrder;
        }
    }
}
