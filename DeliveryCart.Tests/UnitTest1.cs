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
        // [Theory]
        // [InlineData(DatabaseContext.Item.Where(i => i.ItemID = 1), DatabaseContext.Item.Where(i => i.ItemID = 2))]
        // public void Test1(double value1, double value2)
        // {
        
        // }

        [Fact]
        public async Task AddToCart_ItemIsAdded()
        {
            using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
            {
                List<Item> seedItems = DatabaseContext.GetItemsSeed();
                await db.AddRangeAsync(seedItems);
                await db.SaveChangesAsync();

                Item expectedItem = new Item {ItemID = 1, Name = "Milk", Price = 2.99, Status = "In Cart"};

                await db.AddToCart(1);
                //db.Entry().State = EntityState.Detached;

                Item actualItem = db.Item.Where(i => i.ItemID == 1).AsNoTracking().FirstOrDefault();
                //var actualItem = await db.FindAsync<Item>(1);
                Assert.Equal(expectedItem.ItemID, actualItem.ItemID);
            }
        }
        
        // [Fact]
        // public async Task CreateOrder_OrderIsCreated()
        // {
        //     using (var db = new DatabaseContext(Utilities.TestDbContextOptions()))
        //     {
        //         // Arrange
        //         var id = 1;
        //         string name = "John Doe";
        //         string address = "123 Main St";
        //         double total = 1.25;

        //         List<Order> Orders = new List<Order>{
        //             new Order
        //             {
        //                 CustomerName = name,
        //                 CustomerAddress = address,
        //                 OrderTotal = total,
        //                 Status = "Pending"
        //             }
        //         };

        //         List<Item> Items = new List<Item>{
        //             new Item{Name = "Milk", Price = 2.99, Status = "In Cart"},
        //             new Item{Name = "Eggs", Price = 3.99, Status = "In Cart" }
        //         };

        //         List<OrderedItem> orderedItems = new List<OrderedItem>{
        //         new OrderedItem { Item = Items[0], Order = Orders[0] },
        //         new OrderedItem { Item = Items[1], Order = Orders[0] }
        //         };

        //         var expectedOrder = new Order() { OrderID = id, CustomerName = name, CustomerAddress = address, OrderTotal = total, Status = "Pending", OrderedItems = orderedItems };

        //         // Act
        //         db.Item.AddRange(Items);
        //         await db.CreateOrder(name, address, total);

        //         // Assert
        //         var actualOrder = await db.FindAsync<Order>(id);
        //         Assert.Equal(expectedOrder.OrderID, actualOrder.OrderID);
        //     }
        //}
    }
