namespace TrueCode.Finance.Application.Dto
{
    public record CurrencyDto
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public decimal Rate { get; init; }
    }
}
