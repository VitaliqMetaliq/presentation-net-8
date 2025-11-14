using Microsoft.EntityFrameworkCore;

namespace TrueCode.DbMigrator.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new AppDbContextFactory();
            using var context = factory.CreateDbContext(args);
            context.Database.Migrate();
        }
    }
}
