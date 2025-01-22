using ExampleProject4.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleProject4.Infrastructure
{
    public class ExampleDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
