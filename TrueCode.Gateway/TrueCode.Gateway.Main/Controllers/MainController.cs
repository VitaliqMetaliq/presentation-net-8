using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrueCode.Gateway.Application;
using TrueCode.Gateway.Application.Abstractions;

namespace TrueCode.Gateway.Main.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/grpc")]
    public class MainController : ControllerBase
    {
        private readonly IUserFavoritesClient _favoritesClient;

        public MainController(IUserFavoritesClient favoritesClient)
        {
            _favoritesClient = favoritesClient;
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> MakeGrpcCallAsync(CancellationToken cancellationToken)
        {
            var uid = HttpContext.User.FindFirst(ApplicationConstants.Auth.UidClaim)?.Value;
            if (string.IsNullOrWhiteSpace(uid) || !Guid.TryParse(uid, out var parsedUid))
                return Unauthorized();

            var result = await _favoritesClient.GetUserFavoriteCurrencies(uid, cancellationToken);
            return Ok(result);
        }
    }
}
