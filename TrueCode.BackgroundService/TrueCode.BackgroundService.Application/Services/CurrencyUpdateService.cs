using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.BackgroundService.Application.Services
{
    internal class CurrencyUpdateService : ICurrencyUpdateService
    {
        private readonly ICurrencySource _currencySource;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyUpdateService(
            ICurrencySource currencySource, 
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork)
        {
            _currencySource = currencySource;
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateRatesAsync(CancellationToken cancellationToken = default)
        {
            var rates = await _currencySource.FetchRatesAsync(cancellationToken);
            var currencies = await _currencyRepository.GetAllAsync(cancellationToken);
            if (!currencies.Any())
            {
                var entities = rates.Select(e => new CurrencyEntity
                {
                    Id = e.Key,
                    Name = e.Value.Name,
                    Rate = e.Value.Rate
                });

                await _currencyRepository.AddRangeAsync(entities, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                foreach (var rate in rates)
                {
                    if (currencies.TryGetValue(rate.Key, out CurrencyEntity? currency))
                    {
                        currency.Rate = rate.Value.Rate;
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
