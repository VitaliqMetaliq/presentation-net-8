namespace TrueCode.Gateway.Application.Abstractions
{
    public interface IUserFavoritesClient
    {
        Task<IReadOnlyCollection<string>> GetUserFavoriteCurrencies(string uid, CancellationToken cancellationToken = default);
    }
}
