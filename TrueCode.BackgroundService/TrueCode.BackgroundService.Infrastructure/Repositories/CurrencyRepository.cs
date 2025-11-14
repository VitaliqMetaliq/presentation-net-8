using Microsoft.EntityFrameworkCore;
using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.Shared.Infrastructure;

namespace TrueCode.BackgroundService.Infrastructure.Repositories
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _dbContext;

        public CurrencyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task AddRangeAsync(IEnumerable<CurrencyEntity> currencies, CancellationToken cancellationToken = default)
        {
            await _dbContext.Currencies.AddRangeAsync(currencies, cancellationToken);
        }

        public async Task<IDictionary<string, CurrencyEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Currencies.ToDictionaryAsync(e => e.Id, e => e, cancellationToken);
        }
    }
}
