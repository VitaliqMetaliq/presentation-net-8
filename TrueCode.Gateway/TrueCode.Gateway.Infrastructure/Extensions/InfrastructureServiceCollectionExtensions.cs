using Microsoft.Extensions.DependencyInjection;
using TrueCode.Gateway.Application.Abstractions;
using TrueCode.Gateway.Infrastructure.Grpc;

namespace TrueCode.Gateway.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserFavoritesClient, UserFavoritesClient>();
            return services;
        }
    }
}
