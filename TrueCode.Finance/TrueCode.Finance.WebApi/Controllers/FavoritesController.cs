using Microsoft.AspNetCore.Mvc;
using TrueCode.Finance.Application;
using TrueCode.Finance.Application.Abstractions;

namespace TrueCode.Finance.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FavoritesController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavoriteCurrenciesAsync(CancellationToken cancellationToken)
        {
            var userId = Request.Headers[ApplicationConstants.Auth.UserIdHeader].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedId)) 
                return Unauthorized();

            var currencies = await _financeService.GetFavoriteCurrenciesAsync(parsedId, cancellationToken);
            return Ok(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavoritesAsync([FromBody] string[] currencies, CancellationToken cancellationToken)
        {
            var userId = Request.Headers[ApplicationConstants.Auth.UserIdHeader].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedId))
                return Unauthorized();

            await _financeService.AddFavoritesAsync(parsedId, currencies, cancellationToken);

            return Ok();
        }
    }
}
