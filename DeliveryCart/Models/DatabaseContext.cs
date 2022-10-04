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

        public async virtual Task<List<Item>> ListItems()
        {
            return await Item.ToListAsync();
        }

        public async virtual Task AddToCart(int id)
        {
            Item item = new Item() { ItemID = id, Status = "In Cart" };
            Item.Attach(item).Property(i => i.Status).IsModified = true;
            await SaveChangesAsync();
        }

        public async virtual Task<Item> ItemDetails(int id)
        {
            return await Item.FirstOrDefaultAsync(i => i.ItemID == id);
        }

        public async virtual Task<List<Item>> ListCartItems()
        {
            return await Item.Where(i => i.Status == "In Cart").ToListAsync();
        }

        // public static double OrderTotal()
        // {
        //     List<Item> Items = Item.Where(i => i.Status == "In Cart").ToList();

        //     public double total = 0;

        //     foreach(var item in Items){
        //         ot += item.Price;
        //     }

        //     return total;
        // }

        public async virtual Task CreateOrder(string cn, string ca, double ot)
        {
            List<Item> Items = Item.Where(i => i.Status == "In Cart").ToList();
            
            Order OrderToAdd = new Order { CustomerName = cn, CustomerAddress = ca, OrderTotal = ot, Status = "Pending" };

            foreach(var item in Items)
            {
                OrderedItem orderedItem = new OrderedItem { Item = item, Order = OrderToAdd };

                OrderedItems.Add(orderedItem);
            }

            await Order.AddAsync(OrderToAdd);
            await SaveChangesAsync();
        }

        public async virtual Task ClearCart()
        {
            List<Item> Items = Item.Where(i => i.Status == "In Cart").ToList();
            Items.ForEach(i => i.Status="Not in Cart");
            await SaveChangesAsync();
        }
    }

}