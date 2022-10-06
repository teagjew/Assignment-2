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

        public static List<Item> GetItemsSeed()
        {
            return new List<Item> { 
                new Item{Name = "Milk", Price = 2.99, Status = "Not in Cart"},
                new Item{Name = "Eggs", Price = 3.99, Status = "Not in Cart" },
                new Item{Name = "Cheese", Price = 8.29, Status = "Not in Cart" },
                new Item{Name = "Soap", Price = 14.50, Status = "Not in Cart" },
                new Item{Name = "Bread", Price = 5.00, Status = "Not in Cart" },
                new Item{Name = "Water", Price = 1.25, Status = "Not in Cart" },
                new Item{Name = "Soda", Price = 8.75, Status = "Not in Cart" }
            };
        }

        public static List<OrderedItem> GetOrderedItemsSeed(List<Item> Items, List<Order> Orders)
        {
            return new List<OrderedItem>{
                new OrderedItem { Item = Items[0], Order = Orders[0] },
                new OrderedItem { Item = Items[1], Order = Orders[0] },
                new OrderedItem { Item = Items[3], Order = Orders[1] },
                new OrderedItem { Item = Items[5], Order = Orders[1] },
                new OrderedItem { Item = Items[5], Order = Orders[2] },
                new OrderedItem { Item = Items[6], Order = Orders[2] },
            };
        }

        public static List<Order> GetOrdersSeed()
        {
            return new List<Order> {
                new Order
                    {
                        CustomerName = "Jane Smith",
                        CustomerAddress = "123 S 5th St, Lubbock, TX 79540",
                        OrderTotal = 6.98,
                        Status = "Pending"
                    },

                    new Order
                    {
                        CustomerName = "John Doe",
                        CustomerAddress = "555 N 20th St, Lubbock, TX 79152",
                        OrderTotal = 15.75,
                        Status = "Pending"
                    },

                    new Order
                    {
                        CustomerName = "Tim Johns",
                        CustomerAddress = "888 77th St, Lubbock, TX 79525",
                        OrderTotal = 10.00,
                        Status = "Pending"
                    }
            };
        }

        public void SeedDatabase()
        {
            if (Item.Any() && Order.Any() && OrderedItems.Any())
            {
                return;
            }

            var Items = GetItemsSeed();
            var Orders = GetOrdersSeed();

            Item.AddRange(Items);
            Order.AddRange(Orders);
            OrderedItems.AddRange(GetOrderedItemsSeed(Items, Orders));

            SaveChanges();   
        }

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