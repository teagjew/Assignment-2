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

        public List<OrderedItem> OrderedItems { get; set; }

        [BindProperty]
        public int ItemToDelete { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        [BindProperty]
        public string CustomerAddress { get; set; }

        public void OnGet()
        {
            Items = _context.Item.Where(i => i.Status == "In Cart").ToList();
        }

        public async Task<IActionResult> OnPostCreateOrderAsync()
        {
            double ot = 0;
            foreach(var item in Items)
            {
                ot += item.Price;

                OrderedItem orderedItem = new OrderedItem { ItemID = item.ItemID , Item = item};

                OrderedItems.Add(orderedItem);
            }

            Order OrderToAdd = new Order { CustomerName = CustomerName, CustomerAddress = CustomerAddress, OrderTotal = ot, OrderedItems = OrderedItems };

            _context.Order.Add(OrderToAdd);
            _context.SaveChanges();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int id){
            
            Item item = new Item() { ItemID = id, Status = "Not in Cart" };
            
            using(_context)
            {
                _context.Item.Attach(item).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }

            return Page();
        }
    }
}
