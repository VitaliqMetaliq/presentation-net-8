using Microsoft.Extensions.Logging;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.UserService.Application.Abstractions;
using TrueCode.UserService.Application.Exceptions;

namespace TrueCode.UserService.Application.Managers
{
    internal class AuthManager : IAuthManager
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(
            IPasswordHasher passwordHasher, 
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IJwtTokenProvider jwtTokenProvider,
            ILogger<AuthManager> logger)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task RegisterAsync(string userName, string password, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(userName, password, cancellationToken);

            var hash = _passwordHasher.Hash(password);
            var userEntity = new UserEntity()
            {
                Name = userName,
                PasswordHash = hash
            };

            await _userRepository.CreateUserAsync(userEntity, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<string> VerifyAndGenerateTokenAsync(string userName, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
            if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
            {
                throw new AccessDeniedException("Wrong username or password");
            }

            var token = _jwtTokenProvider.GenerateJwtToken(user);
            return token;
        }

        private async Task ValidateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
            if (existing != null)
            {
                _logger.LogWarning("User with UserName = {UserName} already exist", userName);
                throw new ValidationException("User already exist");
            }

            if (password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters.");

            if (!password.Any(char.IsUpper))
                throw new ValidationException("Password must contain an uppercase letter.");

            if (!password.Any(char.IsDigit))
                throw new ValidationException("Password must contain a digit.");
        }
    }
}
