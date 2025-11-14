using FavoritesProto;
using Grpc.Core;
using TrueCode.Finance.Application.Abstractions;

namespace TrueCode.Finance.WebApi.Grpc
{
    public class FavoritesService : Favorites.FavoritesBase
    {
        private readonly IFavoriteCurrencyRepository _repository;
        private readonly ILogger<FavoritesService> _logger;

        public FavoritesService(IFavoriteCurrencyRepository repository, ILogger<FavoritesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<GetUserFavoriteCurrenciesReply> GetUserFavoriteCurrencies(GetUserFavoriteCurrenciesRequest request, ServerCallContext context)
        {
            var uidHeader = context.RequestHeaders.FirstOrDefault(h => h.Key == "x-user-id")?.Value;

            _logger.LogInformation($"gRPC server: received x-user-id = [{uidHeader}]");

            if (uidHeader == null || !Guid.TryParse(uidHeader, out var userId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing or invalid x-user-id"));

            var currencies = await _repository.GetUserFavoriteCurrenciesAsync(userId);

            var reply = new GetUserFavoriteCurrenciesReply();
            reply.Currencies.AddRange(currencies.Select(c => c.Id));

            return reply;
        }
    }
}
