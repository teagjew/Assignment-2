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
            Item Milk = new Item{Name = "Milk", Price = 2.99};
            Item Eggs = new Item{Name = "Eggs", Price = 3.99};
            Item Cheese = new Item{Name = "Cheese", Price = 8.29};
            Item Soap = new Item{Name = "Soap", Price = 14.50};
            Item Bread = new Item{Name = "Bread", Price = 5.00};
            Item Water = new Item{Name = "Water", Price = 1.25};
            Item Soda = new Item{Name = "Soda", Price = 8.75};

            using (var db = new OrderContext(serviceProvider.GetRequiredService<DbContextOptions<OrderContext>>()))
            {
                if (db.Order.Any() && db.Item.Any())
                {
                    return;
                }

                db.Item.AddRange(Milk, Eggs, Cheese, Soap, Bread, Water, Soda);

                db.Order.AddRange(
                    new Order
                    {
                        CustomerName = "Jane Smith",
                        CustomerAddress = "123 S 5th St, Lubbock, TX 79540",
                        OrderTotal = 7.99,
                        Items = new List<Item> {Milk, Bread}

                    },

                    new Order
                    {
                        CustomerName = "John Doe",
                        CustomerAddress = "555 N 20th St, Lubbock, TX 79152",
                        OrderTotal = 18.29,
                        Items = new List<Item> {Soda, Water, Cheese}

                    },

                    new Order
                    {
                        CustomerName = "Tim Johns",
                        CustomerAddress = "888 77th St, Lubbock, TX 79525",
                        OrderTotal = 6.25,
                        Items = new List<Item> {Bread, Water}

                    }
                );

                db.SaveChanges();   
            }
        }
    }
}