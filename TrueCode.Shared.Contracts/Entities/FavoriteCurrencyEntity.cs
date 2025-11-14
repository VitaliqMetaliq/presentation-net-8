namespace TrueCode.Shared.Contracts.Entities
{
    public class FavoriteCurrencyEntity
    {
        public Guid UserId { get; set; }
        public string CurrencyId { get; set; } = string.Empty;

        public UserEntity User { get; set; } = null!;
        public CurrencyEntity Currency { get; set; } = null!;
    }
}
