using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using GameStore.Controllers;
using GameStore.Services;
using GameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace GameStore.Tests.Controllers
{
    public class GameStoreControllerTests
    {
        private readonly Mock<GameStoreService> _mockService;
        private readonly Mock<IValidator<decimal>> _mockDiscountValidator;
        private readonly Mock<IValidator<OrderRequest>> _mockOrderValidator;
        private readonly GameStoreController _controller;

        public GameStoreControllerTests()
        {
            _mockService = new Mock<GameStoreService>(null, null);
            _mockDiscountValidator = new Mock<IValidator<decimal>>();
            _mockOrderValidator = new Mock<IValidator<OrderRequest>>();
            _controller = new GameStoreController(_mockService.Object, _mockDiscountValidator.Object, _mockOrderValidator.Object);
        }

        [Fact]
        public async Task GetGamesWithDiscount_ShouldReturnOkResult_WithValidPercentage()
        {
            // Arrange
            var games = new List<Game> { new Game { Id = "1", Name = "Game A", Price = 90 } };
            _mockService.Setup(s => s.GetGamesWithDiscount(10)).ReturnsAsync(games);
            _mockDiscountValidator.Setup(v => v.ValidateAsync(It.IsAny<decimal>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.GetGamesWithDiscount(10);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(games);
        }

        [Fact]
        public async Task GetGamesWithDiscount_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Discount", "Invalid discount") };
            _mockDiscountValidator.Setup(v => v.ValidateAsync(It.IsAny<decimal>(), default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.GetGamesWithDiscount(150);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new[] { "Invalid discount" });
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnCreatedResult_WithValidRequest()
        {
            // Arrange
            var orderRequest = new OrderRequest { GameId = "123", CustomerName = "John" };
            var order = new Order { Id = "456", GameId = "123", CustomerName = "John", OrderDate = System.DateTime.UtcNow };

            _mockOrderValidator.Setup(v => v.ValidateAsync(It.IsAny<OrderRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            _mockService.Setup(s => s.PlaceOrder(orderRequest.GameId, orderRequest.CustomerName))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>()
                .Which.Value.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("CustomerName", "Name is required") };
            _mockOrderValidator.Setup(v => v.ValidateAsync(It.IsAny<OrderRequest>(), default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.PlaceOrder(new OrderRequest());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new[] { "Name is required" });
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var orderRequest = new OrderRequest { GameId = "123", CustomerName = "John" };
            _mockOrderValidator.Setup(v => v.ValidateAsync(It.IsAny<OrderRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            _mockService.Setup(s => s.PlaceOrder(orderRequest.GameId, orderRequest.CustomerName))
                .ThrowsAsync(new System.Exception("Game not found"));

            // Act
            var result = await _controller.PlaceOrder(orderRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { Message = "Game not found" });
        }
    }
}
