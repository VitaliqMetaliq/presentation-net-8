using TrueCode.Shared.Infrastructure;
using TrueCode.UserService.Application.Abstractions;

namespace TrueCode.UserService.Infrastructure.Repositories
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
