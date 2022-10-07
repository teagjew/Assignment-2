using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assignment_2.Pages;
using Assignment_2.Models;

namespace DeliveryCart.Tests;

    public class UnitTest1
    {
        [Fact]
        public async Task AddToCart_ItemIsAdded()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                await db.AddRangeAsync(seedItems);
                await db.SaveChangesAsync();

                Item expectedItem = new Item {ItemID = 1, Name = "Milk", Price = 2.99, Status = "In Cart"};

                db.ChangeTracker.Clear();
                await db.AddToCart(1);

                Item actualItem = db.Item.Where(i => i.ItemID == 1).AsNoTracking().FirstOrDefault();
                Assert.Equal(expectedItem.Status, actualItem.Status);
            }
        }
        
        [Fact]
        public async Task ClearCart_CartIsCleared()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                await db.AddRangeAsync(seedItems);
                await db.SaveChangesAsync(); 
                db.ChangeTracker.Clear();
                await db.AddToCart(1);

                db.ChangeTracker.Clear();
                await db.ClearCart();

                List<Item> actualItem = db.Item.Where(i => i.Status == "In Cart").ToList();
                Assert.Equal(0, actualItem.Count());
            }
        }

        [Fact]
        public async Task RemoveCartItem_ItemIsRemoved()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                await db.AddRangeAsync(seedItems);
                await db.SaveChangesAsync(); 
                db.ChangeTracker.Clear();
                await db.AddToCart(1);

                Item expectedItem = new Item {ItemID = 1, Name = "Milk", Price = 2.99, Status = "Not in Cart"};

                db.ChangeTracker.Clear();
                await db.RemoveCartItem(1);

                Item actualItem = db.Item.Where(i => i.ItemID == 1).AsNoTracking().FirstOrDefault();
                Assert.Equal(expectedItem.Status, actualItem.Status);
            }
        }

        [Fact]
        public async Task AcceptOrder_OrderAccepted()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Order> seedOrders = DatabaseContext.GetOrdersSeed();
                await db.AddRangeAsync(seedOrders);
                await db.SaveChangesAsync(); 

                Order expectedOrder = new Order { CustomerName = "Jane Smith", CustomerAddress = "123 S 5th St, Lubbock, TX 79540", OrderTotal = 6.98, Status = "Accepted"};

                db.ChangeTracker.Clear();
                await db.AcceptOrder(1);

                Order actualOrder = db.Order.Where(i => i.OrderID == 1).AsNoTracking().FirstOrDefault();
                Assert.Equal(expectedOrder.Status, actualOrder.Status);
            }
        }

        [Fact]
        public async Task DeclineOrder_OrderDeclined()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Order> seedOrders = DatabaseContext.GetOrdersSeed();
                await db.AddRangeAsync(seedOrders);
                await db.SaveChangesAsync(); 

                Order expectedOrder = new Order { CustomerName = "Jane Smith", CustomerAddress = "123 S 5th St, Lubbock, TX 79540", OrderTotal = 6.98, Status = "Declined"};

                db.ChangeTracker.Clear();
                await db.DeclineOrder(1);

                Order actualOrder = db.Order.Where(i => i.OrderID == 1).AsNoTracking().FirstOrDefault();
                Assert.Equal(expectedOrder.Status, actualOrder.Status);
            }
        }

        [Fact]
        public async Task UpdateStatus_StatusUpdated()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Order> seedOrders = DatabaseContext.GetOrdersSeed();
                await db.AddRangeAsync(seedOrders);
                await db.SaveChangesAsync(); 

                Order expectedOrder = new Order { CustomerName = "Jane Smith", CustomerAddress = "123 S 5th St, Lubbock, TX 79540", OrderTotal = 6.98, Status = "In Progress"};

                db.ChangeTracker.Clear();
                await db.UpdateStatus(1, "In Progress");

                Order actualOrder = db.Order.Where(i => i.OrderID == 1).AsNoTracking().FirstOrDefault();
                Assert.Equal(expectedOrder.Status, actualOrder.Status);
            }
        }

        [Fact]
        public async Task CartTotal_ReturnsTotal()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                await db.AddRangeAsync(seedItems);
                await db.SaveChangesAsync(); 
                db.ChangeTracker.Clear();
                await db.AddToCart(1);
                db.ChangeTracker.Clear();
                await db.AddToCart(2);
                db.ChangeTracker.Clear();

                double expectedTotal = 6.98;

                double actualTotal = db.CartTotal();
                Assert.Equal(expectedTotal, actualTotal); 
            }
        }

        [Fact]
        public async Task CreateOrder_OrderIsCreated()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                // Arrange
                var id = 1;
                string name = "John Doe";
                string address = "123 Main St";
                double total = 1.25;

                List<Order> Orders = new List<Order>{
                    new Order
                    {
                        CustomerName = name,
                        CustomerAddress = address,
                        OrderTotal = total,
                        Status = "Pending"
                    }
                };

                List<Item> Items = new List<Item>{
                    new Item{Name = "Milk", Price = 2.99, Status = "In Cart"},
                    new Item{Name = "Eggs", Price = 3.99, Status = "In Cart" }
                };

                List<OrderedItem> orderedItems = new List<OrderedItem>{
                new OrderedItem { Item = Items[0], Order = Orders[0] },
                new OrderedItem { Item = Items[1], Order = Orders[0] }
                };

                var expectedOrder = new Order() { OrderID = id, CustomerName = name, CustomerAddress = address, OrderTotal = total, Status = "Pending", OrderedItems = orderedItems };

                // Act
                db.Item.AddRange(Items);
                await db.CreateOrder(name, address, total);

                // Assert
                var actualOrder = await db.FindAsync<Order>(id);
                Assert.Equal(expectedOrder.OrderID, actualOrder.OrderID);
            }
        }

        [Fact]
        public async Task RemoveOrderItem_ItemIsRemoved()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                List<Order> seedOrders = DatabaseContext.GetOrdersSeed();
                List<OrderedItem> seedOrderedItems = DatabaseContext.GetOrderedItemsSeed(seedItems, seedOrders);
                await db.AddRangeAsync(seedItems);
                await db.AddRangeAsync(seedOrders);
                await db.AddRangeAsync(seedOrderedItems);
                await db.SaveChangesAsync();

                List<OrderedItem> expectedOrderedItems = seedOrderedItems.ToList();
                expectedOrderedItems.RemoveAt(0);

                db.ChangeTracker.Clear();
                await db.RemoveOrderItem(1,1,6.98);

                List<OrderedItem> actualOrderedItems = await db.OrderedItems.AsNoTracking().ToListAsync();
                Assert.Equal(expectedOrderedItems.Count(), actualOrderedItems.Count());
            }
        }
    }
