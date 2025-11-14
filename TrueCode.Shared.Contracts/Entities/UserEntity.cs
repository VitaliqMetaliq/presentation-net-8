namespace TrueCode.Shared.Contracts.Entities
{
    public class UserEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<FavoriteCurrencyEntity> FavoriteCurrencies { get; set; } = [];
    }
}
