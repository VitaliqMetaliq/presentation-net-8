using Microsoft.Extensions.DependencyInjection;
using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.BackgroundService.Infrastructure.Repositories;
using TrueCode.BackgroundService.Infrastructure.Scheduling;
using TrueCode.BackgroundService.Infrastructure.Services;

namespace TrueCode.BackgroundService.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string cbrRequestUrl)
        {
            services.AddHttpClient();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrencySource>(provider =>
            {
                return new CbrCurrencySource(provider.GetRequiredService<IHttpClientFactory>(), 
                    cbrRequestUrl);
            });

            services.AddHostedService<UpdateCurrencyBackgroundService>();
            return services;
        }
    }
}
