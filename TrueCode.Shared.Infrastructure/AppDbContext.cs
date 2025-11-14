using Microsoft.EntityFrameworkCore;
using TrueCode.Shared.Contracts.Entities;
using TrueCode.Shared.Infrastructure.Configuration;

namespace TrueCode.Shared.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<FavoriteCurrencyEntity> FavoriteCurrencies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteCurrencyEntityTypeConfiguration());
        }
    }
}
