using Microsoft.AspNetCore.Identity;
using TrueCode.UserService.Application.Abstractions;

namespace TrueCode.UserService.Infrastructure.Auth
{
    internal class AspNetPasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string password) => _hasher.HashPassword(null!, password);

        public bool Verify(string password, string hash) 
            => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
    }
}
