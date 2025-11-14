namespace TrueCode.BackgroundService.Application.Dto
{
    public record CurrencyDto
    {
        public string Name { get; init; } = string.Empty;
        public decimal Rate { get; init; }
    }
}
