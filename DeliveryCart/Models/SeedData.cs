using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;

namespace Assignment_2.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            List<Item> Items = new List<Item> { 
                new Item{Name = "Milk", Price = 2.99, Status = "Not in Cart"},
                new Item{Name = "Eggs", Price = 3.99, Status = "Not in Cart" },
                new Item{Name = "Cheese", Price = 8.29, Status = "Not in Cart" },
                new Item{Name = "Soap", Price = 14.50, Status = "Not in Cart" },
                new Item{Name = "Bread", Price = 5.00, Status = "Not in Cart" },
                new Item{Name = "Water", Price = 1.25, Status = "Not in Cart" },
                new Item{Name = "Soda", Price = 8.75, Status = "Not in Cart" }
            };

            List<Order> Orders = new List<Order> {
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

            List<OrderedItem> orderedItems = new List<OrderedItem>{
                new OrderedItem { Item = Items[0], Order = Orders[0] },
                new OrderedItem { Item = Items[1], Order = Orders[0] },
                new OrderedItem { Item = Items[3], Order = Orders[1] },
                new OrderedItem { Item = Items[5], Order = Orders[1] },
                new OrderedItem { Item = Items[5], Order = Orders[2] },
                new OrderedItem { Item = Items[6], Order = Orders[2] },
            };

            using (var db = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                if (db.Item.Any() && db.Order.Any() && db.OrderedItems.Any())
                {
                    return;
                }

                db.Item.AddRange(Items);
                db.Order.AddRange(Orders);
                db.OrderedItems.AddRange(orderedItems);

                db.SaveChanges();   
            }
        }
    }
}