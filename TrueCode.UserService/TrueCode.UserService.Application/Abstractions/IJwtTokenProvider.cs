using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.UserService.Application.Abstractions
{
    public interface IJwtTokenProvider
    {
        string GenerateJwtToken(UserEntity user);
    }
}
