using Microsoft.Extensions.DependencyInjection;
using TrueCode.BackgroundService.Application.Abstractions;
using TrueCode.BackgroundService.Application.Services;

namespace TrueCode.BackgroundService.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyUpdateService, CurrencyUpdateService>();
            return services;
        }
    }
}
