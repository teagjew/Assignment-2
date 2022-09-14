using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext (DbContextOptions<OrderContext> options)
            : base(options)
            {
            }
            public DbSet<Order> Order {get; set;}
            public DbSet<Item> Item {get; set;}
    }

}