namespace TrueCode.BackgroundService.Application.Abstractions
{
    public interface ICurrencyUpdateService
    {
        Task UpdateRatesAsync(CancellationToken cancellationToken = default);
    }
}
