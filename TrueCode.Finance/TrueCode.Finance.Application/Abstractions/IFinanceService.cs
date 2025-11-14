using TrueCode.Finance.Application.Dto;

namespace TrueCode.Finance.Application.Abstractions
{
    public interface IFinanceService
    {
        Task<IReadOnlyCollection<CurrencyDto>> GetFavoriteCurrenciesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencies, CancellationToken cancellationToken = default);
    }
}
