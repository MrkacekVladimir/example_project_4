using ExampleProject4.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject4.Infrastructure
{
    public class ExampleDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Pro testovací účely mohu použít jenom SQLite, která je uložena v paměti RAM
            //Samozřejmě po vypnutí aplikace se celá databáze zničí
            //optionsBuilder.UseSqlite(":memory:");

            //Citlivé údaje je důležité přesunout do konfigurace aplikace
            //Např. appsettings.json, user-secrets, environment variables apod...
            string server = "mysqlstudenti.litv.sssvt.cz";
            string database = "mrkacek_db4";
            string username = "mrkacekwrite";
            string password = "123456";

            optionsBuilder.UseMySQL($"server={server};database={database};user={username};password={password}");
        }

    }
}
