using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
using TrueCode.BackgroundService.Application.Extensions;
using TrueCode.BackgroundService.Infrastructure.Extensions;
using TrueCode.BackgroundService.Infrastructure.Scheduling;
using TrueCode.Shared.Infrastructure;

namespace TrueCode.BackgroundService.Main.Extensions
{
    internal static class WebApplicationBuilderExtensions
    {
        internal static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<UpdateCurrencyOptions>(
                builder.Configuration.GetSection(nameof(UpdateCurrencyOptions)));

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.AddSerilog((s, config) =>
            {
                config.ReadFrom.Configuration(builder.Configuration)
                    .ReadFrom.Services(s)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "TrueCode.BackgroundService");
            });

            builder.Services.AddInfrastructureServices(builder.Configuration["CurrencySource:Url"]!);
            builder.Services.AddApplicationServices();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
