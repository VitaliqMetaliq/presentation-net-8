using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using TrueCode.BackgroundService.Application.Abstractions;

namespace TrueCode.BackgroundService.Infrastructure.Scheduling
{
    internal class UpdateCurrencyBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UpdateCurrencyBackgroundService> _logger;
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun = DateTime.MinValue;

        public UpdateCurrencyBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<UpdateCurrencyBackgroundService> logger,
            IOptions<UpdateCurrencyOptions> options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _crontabSchedule = CrontabSchedule.Parse(options.Value.CronExpression);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UpdateCurrencyBackgroundService started. Next run in {NextRun}", _nextRun);
            while(!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                if (now >= _nextRun)
                {
                    try
                    {
                        await using var scope = _serviceProvider.CreateAsyncScope();
                        var currencyUpdateService = scope.ServiceProvider.GetRequiredService<ICurrencyUpdateService>();
                        await currencyUpdateService.UpdateRatesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating currencies.");
                    }

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.UtcNow);
                    _logger.LogInformation("Next run in {NextRun}", _nextRun);
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
