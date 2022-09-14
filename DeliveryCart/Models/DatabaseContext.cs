using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Models
{
    public class DatabaseContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderedItem>().HasKey(oi => new { oi.OrderID, oi.ItemID });
        }

        public DatabaseContext (DbContextOptions<DatabaseContext> options)
            : base(options)
            {
            }
            public DbSet<Order> Order {get; set;}
            public DbSet<Item> Item {get; set;}
            public DbSet<OrderedItem> OrderedItems {get; set;}
    }

}