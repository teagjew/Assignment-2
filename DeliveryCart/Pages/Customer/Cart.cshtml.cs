using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment_2.Models;

namespace Assignment_2.Pages.Customer
{
    public class CartModel: PageModel
    {
        private readonly Assignment_2.Models.DatabaseContext _context;

        public CartModel(Assignment_2.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Item> Items { get; set; }

        public List<OrderedItem> OrderedItems = new List<OrderedItem>();

        [BindProperty]
        public int ItemToDelete { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        [BindProperty]
        public string CustomerAddress { get; set; }

        public double OrderTotal = 0;

        public void OnGet()
        {
            Items = _context.Item.Where(i => i.Status == "In Cart").ToList();

            foreach(var item in Items){
                OrderTotal += item.Price;
            }
        }

        public async Task<IActionResult> OnPostCreateOrderAsync()
        {
            Items = _context.Item.Where(i => i.Status == "In Cart").ToList();

            foreach(var item in Items){
                OrderTotal += item.Price;
            }

            Order OrderToAdd = new Order { CustomerName = CustomerName, CustomerAddress = CustomerAddress, OrderTotal = OrderTotal, Status = "Pending" };

            foreach(var item in Items)
            {
                OrderedItem orderedItem = new OrderedItem { Item = item, Order = OrderToAdd };

                OrderedItems.Add(orderedItem);
            }

            _context.OrderedItems.AddRange(OrderedItems);
            _context.Order.Add(OrderToAdd);
            _context.SaveChanges();

            using(_context)
            {
                var ClearCart = _context.Item.ToList();
                ClearCart.ForEach(i => i.Status="Not in Cart");
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int id){
            
            Item item = new Item() { ItemID = id, Status = "Not in Cart" };
            
            using(_context)
            {
                _context.Item.Attach(item).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }

            return RedirectToPage("./Cart");
        }
    }
}
