using Microsoft.Extensions.DependencyInjection;
using TrueCode.UserService.Application.Abstractions;
using TrueCode.UserService.Infrastructure.Auth;
using TrueCode.UserService.Infrastructure.Repositories;

namespace TrueCode.UserService.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            string issuer, string audience, int tokenLifeTimeMinutes, string secret)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IJwtTokenProvider>(_ => new JwtTokenProvider(issuer, audience, tokenLifeTimeMinutes, secret));
            services.AddSingleton<IPasswordHasher, AspNetPasswordHasher>();
            return services;
        }
    }
}
