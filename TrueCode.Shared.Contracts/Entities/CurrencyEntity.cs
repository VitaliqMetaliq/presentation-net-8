namespace TrueCode.Shared.Contracts.Entities
{
    public class CurrencyEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}
