using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using TrueCode.Shared.Infrastructure;
using TrueCode.UserService.Application.Extensions;
using TrueCode.UserService.Infrastructure.Extensions;
using TrueCode.UserService.WebApi.Middleware;
using TrueCode.UserService.WebApi.Settings;

namespace TrueCode.UserService.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = new AuthSettings();
            Configuration.GetSection(nameof(AuthSettings)).Bind(authSettings);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddSerilog((s, config) =>
            {
                config.ReadFrom.Configuration(Configuration)
                    .ReadFrom.Services(s)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "TrueCode.UserService");
            });

            services.AddInfrastructureServices(authSettings.Issuer, authSettings.Audience, authSettings.TokenLifeTimeMinutes, authSettings.Secret);

            services.AddApplicationServices();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "TrueCode.UserService.WebApi v1",
                    Version = "v1",
                });
            });

            services.AddControllers();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (env.IsDevelopment()) 
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "TrueCode.UserService.WebApi";
                    options.SwaggerEndpoint("v1/swagger.json", "TrueCode.UserService.WebApi");
                });
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
