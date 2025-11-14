namespace TrueCode.UserService.Application.Abstractions
{
    public interface IAuthManager
    {
        Task RegisterAsync(string userName, string password, CancellationToken cancellationToken = default);
        Task<string> VerifyAndGenerateTokenAsync(string userName, string password, CancellationToken cancellationToken = default);
    }
}
