using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TrueCode.Finance.Application.Abstractions;
using TrueCode.Finance.Application.Dto;
using TrueCode.Finance.Application.Services;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Finance.Tests
{
    public class FinanceServiceTests
    {
        private readonly Mock<IFavoriteCurrencyRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<FinanceService>> _loggerMock;
        private readonly FinanceService _financeService;

        public FinanceServiceTests()
        {
            _repositoryMock = new Mock<IFavoriteCurrencyRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<FinanceService>>();
            _financeService = new FinanceService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetFavoriteCurrenciesAsync_WithResults_ReturnsDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var entities = new List<CurrencyEntity> { new() { Id = "USD" } };
            var dtos = new List<CurrencyDto> { new() { Id = "USD" } };

            _repositoryMock.Setup(e => e.GetUserFavoriteCurrenciesAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            _mapperMock.Setup(e => e.Map<List<CurrencyDto>>(entities))
                .Returns(dtos);

            // Act
            var result = await _financeService.GetFavoriteCurrenciesAsync(userId);

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetFavoriteCurrenciesAsync_NoResults_ReturnsEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _repositoryMock.Setup(e => e.GetUserFavoriteCurrenciesAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            // Act
            var result = await _financeService.GetFavoriteCurrenciesAsync(userId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddFavoritesAsync_NewCurrencies_AddsToRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencies = new List<string> { "USD", "EUR" };
            var existing = new HashSet<string> { "UZS" };

            _repositoryMock.Setup(e => e.GetUserFavoriteCurrencyIdsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            // Act
            await _financeService.AddFavoritesAsync(userId, currencies, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(e => e.AddFavoriteCurrenciesAsync(
                userId,
                It.Is<IEnumerable<FavoriteCurrencyEntity>>(e =>
                    e.Count() == 2 && e.All(f => f.UserId == userId)
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddFavoritesAsync_NoNewCurrencies_RepoNotCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencies = new List<string> { "USD" };
            var existing = new HashSet<string> { "USD" };

            _repositoryMock.Setup(r => r.GetUserFavoriteCurrencyIdsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            // Act
            await _financeService.AddFavoritesAsync(userId, currencies);

            // Assert
            _repositoryMock.Verify(r => r.AddFavoriteCurrenciesAsync(
                It.IsAny<Guid>(), 
                It.IsAny<IEnumerable<FavoriteCurrencyEntity>>(), 
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddFavoritesAsync_EmptyCurrencies_RepoNotCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currencies = new List<string>();

            // Act
            await _financeService.AddFavoritesAsync(userId, currencies);

            // Assert
            _repositoryMock.Verify(r => r.GetUserFavoriteCurrencyIdsAsync(
                It.IsAny<Guid>(), 
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
