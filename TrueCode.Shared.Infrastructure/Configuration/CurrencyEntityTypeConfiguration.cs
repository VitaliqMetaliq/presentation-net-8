using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Shared.Infrastructure.Configuration
{
    internal class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<CurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(3);

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.Rate).IsRequired().HasPrecision(18, 4);
        }
    }
}
