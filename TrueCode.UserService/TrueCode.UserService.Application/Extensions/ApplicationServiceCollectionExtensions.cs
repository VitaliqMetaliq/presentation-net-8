using Microsoft.Extensions.DependencyInjection;
using TrueCode.UserService.Application.Abstractions;
using TrueCode.UserService.Application.Managers;

namespace TrueCode.UserService.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager, AuthManager>();
            return services;
        }
    }
}
