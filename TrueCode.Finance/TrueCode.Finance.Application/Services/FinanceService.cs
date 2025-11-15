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
            try
            {
                var existingIds = await _repository.GetExistingCurrencyIdsAsync(currencies, cancellationToken);
                if (existingIds.Count == 0) return;

                var userFavorites = await _repository.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);

                var newFavorites = existingIds
                    .Except(userFavorites)
                    .Select(id => new FavoriteCurrencyEntity
                    {
                        UserId = userId,
                        CurrencyId = id
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
