using FavoritesProto;
using Grpc.Core;
using Grpc.Net.Client;
using TrueCode.Gateway.Application;
using TrueCode.Gateway.Application.Abstractions;

namespace TrueCode.Gateway.Infrastructure.Grpc
{
    internal class UserFavoritesClient : IUserFavoritesClient
    {
        private readonly GrpcChannel _channel;

        public UserFavoritesClient(GrpcChannel channel)
        {
            _channel = channel;
        }

        public async Task<IReadOnlyCollection<string>> GetUserFavoriteCurrencies(string uid, CancellationToken cancellationToken = default)
        {
            var client = new Favorites.FavoritesClient(_channel);

            var request = new GetUserFavoriteCurrenciesRequest();
            var headers = new Metadata();
            if (!string.IsNullOrEmpty(uid))
                headers.Add(ApplicationConstants.Auth.UserIdHeader, uid);

            var call = client.GetUserFavoriteCurrenciesAsync(request, headers, cancellationToken: cancellationToken);

            var response = await call.ResponseAsync;
            Console.WriteLine(response);

            return response.Currencies;
        }
    }
}
