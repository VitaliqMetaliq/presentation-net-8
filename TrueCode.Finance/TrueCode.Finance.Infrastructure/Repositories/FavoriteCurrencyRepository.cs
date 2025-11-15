using Microsoft.EntityFrameworkCore;
using TrueCode.Finance.Application.Abstractions;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.Shared.Infrastructure;

namespace TrueCode.Finance.Infrastructure.Repositories
{
    internal class FavoriteCurrencyRepository : IFavoriteCurrencyRepository
    {
        private readonly AppDbContext _dbContext;

        public FavoriteCurrencyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<CurrencyEntity>> GetUserFavoriteCurrenciesAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.FavoriteCurrencies
                .Where(e => e.UserId == userId)
                .Select(e => e.Currency)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<HashSet<string>> GetUserFavoriteCurrencyIdsAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.FavoriteCurrencies.Where(e => e.UserId == userId)
                .Select(e => e.CurrencyId)
                .AsNoTracking()
                .ToHashSetAsync(cancellationToken);
        }

        public async Task AddFavoriteCurrenciesAsync(
            Guid userId, IEnumerable<FavoriteCurrencyEntity> favorites, CancellationToken cancellationToken = default)
        {
            await _dbContext.FavoriteCurrencies.AddRangeAsync(favorites, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<HashSet<string>> GetExistingCurrencyIdsAsync(
            IReadOnlyCollection<string> currencyIds,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Currencies
                .Where(c => currencyIds.Contains(c.Id))
                .Select(c => c.Id)
                .AsNoTracking()
                .ToHashSetAsync(cancellationToken);
        }
    }
}
