using TrueCode.UserService.Infrastructure.Auth;
using Xunit;

namespace TrueCode.UserService.Tests
{
    public class AspNetPasswordHasherTests
    {
        private readonly AspNetPasswordHasher _hasher = new();

        [Fact]
        public void Hash_Verify_ValidPassword_ReturnsTrue()
        {
            var password = "Passw0rd123";
            var hash = _hasher.Hash(password);

            var result = _hasher.Verify(password, hash);

            Assert.True(result);
        }
    }
}
