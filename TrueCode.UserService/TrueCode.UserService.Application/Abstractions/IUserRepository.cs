using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.UserService.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default);
    }
}
