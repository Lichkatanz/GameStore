using Moq;
using Xunit;
using FluentAssertions;
using GameStore.Repositories;
using GameStore.Services;
using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Tests.Services
{
    public class GameStoreServiceTests
    {
        private readonly Mock<GameRepository> _mockGameRepo;
        private readonly Mock<OrderRepository> _mockOrderRepo;
        private readonly GameStoreService _service;

        public GameStoreServiceTests()
        {
            _mockGameRepo = new Mock<GameRepository>();
            _mockOrderRepo = new Mock<OrderRepository>();
            _service = new GameStoreService(_mockGameRepo.Object, _mockOrderRepo.Object);
        }

        [Fact]
        public async Task GetGamesWithDiscount_ShouldApplyDiscountToAllGames()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = "1", Name = "Game A", Price = 100 },
                new Game { Id = "2", Name = "Game B", Price = 200 }
            };

            _mockGameRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

            // Act
            var result = await _service.GetGamesWithDiscount(10); // 10% discount

            // Assert
            result.Should().HaveCount(2);
            result[0].Price.Should().Be(90); // 100 - 10%
            result[1].Price.Should().Be(180); // 200 - 10%
        }

        [Fact]
        public async Task PlaceOrder_ShouldCreateNewOrder()
        {
            // Arrange
            var gameId = "123";
            var customerName = "John Doe";
            var game = new Game { Id = gameId, Name = "Game X", Price = 50 };

            _mockGameRepo.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync(game);
            _mockOrderRepo.Setup(repo => repo.CreateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.PlaceOrder(gameId, customerName);

            // Assert
            result.Should().NotBeNull();
            result.GameId.Should().Be(gameId);
            result.CustomerName.Should().Be(customerName);
            result.OrderDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task PlaceOrder_ShouldThrowException_WhenGameNotFound()
        {
            // Arrange
            _mockGameRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Game)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.PlaceOrder("999", "John Doe"));
        }
    }
}
