using Microsoft.Extensions.DependencyInjection;
using TrueCode.Finance.Application.Abstractions;
using TrueCode.Finance.Infrastructure.Repositories;

namespace TrueCode.Finance.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IFavoriteCurrencyRepository, FavoriteCurrencyRepository>();
            return services;
        }
    }
}
