using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using TrueCode.Finance.Application.Extensions;
using TrueCode.Finance.Infrastructure.Extensions;
using TrueCode.Finance.WebApi.Grpc;
using TrueCode.Finance.WebApi.Middleware;
using TrueCode.Shared.Infrastructure;

namespace TrueCode.Finance.WebApi
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddSerilog((s, config) =>
            {
                config.ReadFrom.Configuration(Configuration)
                    .ReadFrom.Services(s)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "TrueCode.Finance");
            });

            services.AddInfrastructureServices();
            services.AddApplicationServices();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "TrueCode.Finance.WebApi v1",
                    Version = "v1",
                });
            });

            services.AddControllers();
            services.AddHealthChecks();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "TrueCode.Finance.WebApi";
                    options.SwaggerEndpoint("v1/swagger.json", "TrueCode.Finance.WebApi");
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<FavoritesService>();
            });
        }
    }
}
