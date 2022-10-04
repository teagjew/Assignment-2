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

        public List<Item> Items { get; set; }

        public List<OrderedItem> OrderedItems = new List<OrderedItem>();

        [BindProperty]
        public int ItemToDelete { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        [BindProperty]
        public string CustomerAddress { get; set; }

        public double OrderTotal = 0;

        public async Task OnGet()
        {
            Items = await _context.ListCartItems();

            //OrderTotal = await _context.OrderTotal();
            foreach(var item in Items){
                OrderTotal += item.Price;
            }
        }

        public async Task<IActionResult> OnPostCreateOrderAsync()
        {
            //OrderTotal = await _context.OrderTotal();
            Items = await _context.ListCartItems();
            foreach(var item in Items){
                OrderTotal += item.Price;
            }

            await _context.CreateOrder(CustomerName, CustomerAddress, OrderTotal);

            await _context.ClearCart();

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
