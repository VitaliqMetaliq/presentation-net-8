using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Finance.Application.Abstractions
{
    public interface IFavoriteCurrencyRepository
    {
        Task<IReadOnlyCollection<CurrencyEntity>> GetUserFavoriteCurrenciesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<HashSet<string>> GetUserFavoriteCurrencyIdsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<HashSet<string>> GetExistingCurrencyIdsAsync(IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken = default);
        Task AddFavoriteCurrenciesAsync(Guid userId, IEnumerable<FavoriteCurrencyEntity> favorites, CancellationToken cancellationToken = default);
    }
}
