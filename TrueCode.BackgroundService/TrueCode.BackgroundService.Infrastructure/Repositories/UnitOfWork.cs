using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.Shared.Infrastructure;

namespace TrueCode.BackgroundService.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);
    }
}
