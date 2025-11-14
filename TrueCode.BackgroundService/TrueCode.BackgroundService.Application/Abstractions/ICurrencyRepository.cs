using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.BackgroundService.Application.Abstractions
{
    public interface ICurrencyRepository
    {
        Task AddRangeAsync(IEnumerable<CurrencyEntity> currencies, CancellationToken cancellationToken = default);
        Task<IDictionary<string, CurrencyEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
