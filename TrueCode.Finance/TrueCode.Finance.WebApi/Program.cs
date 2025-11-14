using Serilog;

namespace TrueCode.Finance.WebApi
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();
                
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(config =>
            {
                config.UseStartup<Startup>();
                config.ConfigureKestrel((context, options) =>
                {
                    options.Configure(context.Configuration.GetSection("Kestrel"));
                });
            });
    }
}
