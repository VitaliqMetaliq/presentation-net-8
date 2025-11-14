using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.Finance.Application.Abstractions;
using TrueCode.Finance.Application.Services;

namespace TrueCode.Finance.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFinanceService, FinanceService>();
            services.AddMapster();
            return services;
        } 
    }
}
