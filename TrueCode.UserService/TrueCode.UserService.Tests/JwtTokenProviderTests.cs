using System.IdentityModel.Tokens.Jwt;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.UserService.Infrastructure.Auth;
using Xunit;

namespace TrueCode.UserService.Tests
{
    public class JwtTokenProviderTests
    {
        private readonly JwtTokenProvider _provider = new(
            "test-issuer", "test-audience", 5, "very-long-secret-for-jwt-signing-32bytes");

        [Fact]
        public void GenerateJwtToken_CreatesValidToken()
        {
            var user = new UserEntity { Name = "testUser" };
            var token = _provider.GenerateJwtToken(user);

            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));
        }
    }
}
