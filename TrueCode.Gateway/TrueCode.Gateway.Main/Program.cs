using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TrueCode.Gateway.Infrastructure.Transforms;
using TrueCode.Gateway.Main.Settings;
using Grpc.Net.Client;
using TrueCode.Gateway.Infrastructure.Extensions;

namespace TrueCode.Gateway.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var authSettings = new AuthSettings();
            builder.Configuration.GetSection(nameof(AuthSettings)).Bind(authSettings);

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(2)
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("RequireAuthenticatedUser", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

            builder.Services.AddControllers();
            builder.Services.AddInfrastructureServices();

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms<UidTransformProvider>();

            builder.Services.AddSingleton(s => GrpcChannel.ForAddress(
                builder.Configuration["GrpcAddress"]!,
                new GrpcChannelOptions
                {
                    Credentials = Grpc.Core.ChannelCredentials.Insecure
                }));

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapReverseProxy(configure =>
            {
                configure.UseAuthentication();
                configure.UseAuthorization();
            });

            app.MapControllers();

            app.Run();
        }
    }
}
