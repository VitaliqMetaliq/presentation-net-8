using TrueCode.BackgroundService.Application.Dto;

namespace TrueCode.BackgroundService.Application.Abstractions
{
    public interface ICurrencySource
    {
        Task<IReadOnlyDictionary<string, CurrencyDto>> FetchRatesAsync(CancellationToken cancellationToken = default);
    }
}
