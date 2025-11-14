using MapsterMapper;
using Microsoft.Extensions.Logging;
using TrueCode.Finance.Application.Abstractions;
using TrueCode.Finance.Application.Dto;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Finance.Application.Services
{
    internal class FinanceService : IFinanceService
    {
        private readonly IFavoriteCurrencyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FinanceService> _logger;

        public FinanceService(
            IFavoriteCurrencyRepository repository, 
            IMapper mapper,
            ILogger<FinanceService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<CurrencyDto>> GetFavoriteCurrenciesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entities = await _repository.GetUserFavoriteCurrenciesAsync(userId, cancellationToken);
                if (entities.Count > 0)
                {
                    return _mapper.Map<List<CurrencyDto>>(entities);
                }

                return [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting favorite currencies");
                throw;
            }
        }

        public async Task AddFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencies, CancellationToken cancellationToken = default)
        {
            if (currencies.Count == 0) return;

            try
            {
                var existing = await _repository.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);

                var newFavorites = currencies.Where(e => !existing.Contains(e))
                    .Select(e => new FavoriteCurrencyEntity
                    {
                        UserId = userId,
                        CurrencyId = e
                    });

                if (newFavorites.Any())
                    await _repository.AddFavoriteCurrenciesAsync(userId, newFavorites, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding favorite currencies");
                throw;
            }
        }
    }
}
