using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Shared.Infrastructure.Configuration
{
    internal class FavoriteCurrencyEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteCurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<FavoriteCurrencyEntity> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CurrencyId });

            builder.Property(x => x.CurrencyId).HasMaxLength(3);

            builder.HasOne(x => x.User)
                .WithMany(x => x.FavoriteCurrencies)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);
        }
    }
}
