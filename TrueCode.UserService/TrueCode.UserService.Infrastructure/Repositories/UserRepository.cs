using Microsoft.EntityFrameworkCore;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.Shared.Infrastructure;
using TrueCode.UserService.Application.Abstractions;

namespace TrueCode.UserService.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == userName, cancellationToken);
        }

        public async Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }
    }
}
