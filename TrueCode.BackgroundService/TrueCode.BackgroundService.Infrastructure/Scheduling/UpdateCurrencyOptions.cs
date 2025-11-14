namespace TrueCode.BackgroundService.Infrastructure.Scheduling
{
    public record UpdateCurrencyOptions
    {
        public string CronExpression { get; set; } = "* * * * *";
    }
}
