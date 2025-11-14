using Microsoft.Extensions.Logging;
using Moq;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.UserService.Application.Abstractions;
using TrueCode.UserService.Application.Exceptions;
using TrueCode.UserService.Application.Managers;
using Xunit;

namespace TrueCode.UserService.Tests
{
    public class AuthManagerTests
    {
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtTokenProvider> _jwtTokenProviderMock;
        private readonly Mock<ILogger<AuthManager>> _loggerMock;
        private readonly AuthManager _authManager;

        public AuthManagerTests()
        {
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenProviderMock = new Mock<IJwtTokenProvider>();
            _loggerMock = new Mock<ILogger<AuthManager>>();

            _authManager = new AuthManager(
                _passwordHasherMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _jwtTokenProviderMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ValidInput_CreatesUser()
        {
            // Arrange
            var userName = "testuser";
            var password = "Password1";
            var hashedPassword = "hashed";
            var userEntity = new UserEntity { Name = userName, PasswordHash = hashedPassword };

            _userRepositoryMock.Setup(e => e.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as UserEntity);

            _passwordHasherMock.Setup(e => e.Hash(password))
                .Returns(hashedPassword);

            // Act
            await _authManager.RegisterAsync(userName, password);

            // Assert
            _userRepositoryMock.Verify(e => e.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task VerifyAndGenerateTokenAsync_ValidCredentials_GeneratesToken()
        {
            // Arrange
            var userName = "testuser";
            var password = "Password1";
            var userEntity = new UserEntity { Name = userName, PasswordHash = "hashed" };
            var token = "jwt-token";

            _userRepositoryMock
                .Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userEntity);

            _passwordHasherMock
                .Setup(hasher => hasher.Verify(password, userEntity.PasswordHash))
                .Returns(true);

            _jwtTokenProviderMock
                .Setup(provider => provider.GenerateJwtToken(userEntity))
                .Returns(token);

            // Act
            var result = await _authManager.VerifyAndGenerateTokenAsync(userName, password, CancellationToken.None);

            // Assert
            Assert.Equal(token, result);
        }

        [Fact]
        public async Task VerifyAndGenerateTokenAsync_InvalidCredentials_ThrowsAccessDeniedException()
        {
            // Arrange
            var userName = "testuser";
            var password = "wrongpassword";

            _userRepositoryMock
                .Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserEntity { Name = userName, PasswordHash = "hashed" });

            _passwordHasherMock
                .Setup(hasher => hasher.Verify(password, It.IsAny<string>()))
                .Returns(false);

            // Act / Assert
            await Assert.ThrowsAsync<AccessDeniedException>(
                () => _authManager.VerifyAndGenerateTokenAsync(userName, password, CancellationToken.None));
        }

        [Theory]
        [InlineData("", "ValidPass1", "User already exist")]
        [InlineData("test", "123", "Password must be at least 8 characters.")]
        [InlineData("test", "badpassword", "Password must contain an uppercase letter.")]
        [InlineData("test", "Badpassword", "Password must contain a digit.")]
        public async Task RegisterAsync_InvalidInput_ThrowsValidationException(
            string userName, string password, string expectedMessage)
        {
            // Arrange
            _userRepositoryMock
                .Setup(repo => repo.GetByUserNameAsync(userName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userName == "" ? new UserEntity() : null);

            // Act / Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => _authManager.RegisterAsync(userName, password, CancellationToken.None));

            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}
