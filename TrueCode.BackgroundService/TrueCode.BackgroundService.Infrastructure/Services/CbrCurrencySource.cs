using System.Xml.Linq;
using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.BackgroundService.Application.Dto;

namespace TrueCode.BackgroundService.Infrastructure.Services
{
    internal class CbrCurrencySource : ICurrencySource
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _ratesUrl;

        public CbrCurrencySource(IHttpClientFactory httpClientFactory, string ratesUrl)
        {
            _httpClientFactory = httpClientFactory;
            _ratesUrl = ratesUrl;
        }

        public async Task<IReadOnlyDictionary<string, CurrencyDto>> FetchRatesAsync(CancellationToken cancellationToken = default)
        {
            using var client = _httpClientFactory.CreateClient();
            var xmlString = await client.GetStringAsync(_ratesUrl, cancellationToken);

            if (!string.IsNullOrWhiteSpace(xmlString))
            {
                var doc = XDocument.Parse(xmlString);

                return doc.Descendants("Valute")
                    .ToDictionary(
                        v => v.Element("CharCode")?.Value ?? string.Empty,
                        v =>
                        {
                            var valueStr = v.Element("Value")!.Value.Replace(",", ".");
                            var nominalStr = v.Element("Nominal")!.Value;
                            return new CurrencyDto { 
                                Name = v.Element("Name")?.Value ?? string.Empty,
                                Rate = decimal.Parse(valueStr) / decimal.Parse(nominalStr)
                            };
                        });
            }

            return new Dictionary<string, CurrencyDto>();
        }
    }
}
