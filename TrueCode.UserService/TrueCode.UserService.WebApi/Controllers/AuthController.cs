using Microsoft.AspNetCore.Mvc;
using TrueCode.UserService.Application.Abstractions;
using TrueCode.UserService.WebApi.Dto;

namespace TrueCode.UserService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private const string EmptyCredentialsMessage = "Username and password required";

        public AuthController(IAuthManager userManager)
        {
            _authManager = userManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            if (!ValidateEmptyCredentials(request.UserName, request.Password))
            {
                return BadRequest(EmptyCredentialsMessage);
            }

            await _authManager.RegisterAsync(request.UserName!, request.Password!, cancellationToken);
            return Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            if (!ValidateEmptyCredentials(request.UserName, request.Password))
            {
                return BadRequest(EmptyCredentialsMessage);
            }

            var token = await _authManager.VerifyAndGenerateTokenAsync(request.UserName!, request.Password!, cancellationToken);

            return Ok(token);
        }

        private static bool ValidateEmptyCredentials(string? userName, string? password) =>
            !(string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password));
    }
}
