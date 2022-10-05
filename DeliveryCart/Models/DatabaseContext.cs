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

        public virtual double CartTotal()
        {
            var Items = Item.Where(i => i.Status == "In Cart").ToList();
            double total = 0;
            foreach(var item in Items){
                total += item.Price;
            }
            return total;
        }

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

        public async virtual Task RemoveCartItem(int id)
        {
            Item item = new Item() { ItemID = id, Status = "Not in Cart" };
            Item.Attach(item).Property(i => i.Status).IsModified = true;
            SaveChangesAsync();
        }

        public async virtual Task<List<Order>> ListOrders()
        {
            return await Order.ToListAsync();
        }

        public async virtual Task AcceptOrder(int id)
        {
            Order order = new Order() { OrderID = id, Status = "Accepted" };
            Order.Attach(order).Property(i => i.Status).IsModified = true;
            SaveChangesAsync();
        }

        public async virtual Task DeclineOrder(int id)
        {
            Order order = new Order() { OrderID = id, Status = "Declined" };
            Order.Attach(order).Property(i => i.Status).IsModified = true;
            SaveChangesAsync();
        }

        public async virtual Task UpdateStatus(int id, string status)
        {
            Order order = new Order() { OrderID = id, Status = status };
            Order.Attach(order).Property(i => i.Status).IsModified = true;
            SaveChangesAsync();
        }

        public async virtual Task<Order> OrderDetails(int id)
        {
            return await Order.Include(o => o.OrderedItems).ThenInclude(oi => oi.Item).FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async virtual Task RemoveOrderItem(int oid, int iid, double total)
        {
            OrderedItem OrderedItem = OrderedItems.FirstOrDefault(o => (o.OrderID == oid) && (o.ItemID == iid));
            Item ItemBeingDeleted = Item.FirstOrDefault(i => i.ItemID == iid);

            double NewTotal = total - ItemBeingDeleted.Price;
            
            if (OrderedItem != null)
            {
                OrderedItems.Remove(OrderedItem);
                SaveChangesAsync();

                var UpdatedOrder = Order.Where(o => o.OrderID == oid).ToList();
                UpdatedOrder.ForEach(o => o.OrderTotal=NewTotal);
                SaveChangesAsync();
            }
        }
    }
}