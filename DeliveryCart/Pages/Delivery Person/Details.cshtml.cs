using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment_2.Models;

namespace Assignment_2.Pages.DeliveryPerson
{
    public class DetailsModel : PageModel
    {
        private readonly Assignment_2.Models.DatabaseContext _context;

        public DetailsModel(Assignment_2.Models.DatabaseContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        [BindProperty]
        public int ItemIDToDelete {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Order.Include(m => m.OrderedItems).ThenInclude(oi => oi.Item).FirstOrDefaultAsync(m => m.OrderID == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostDeleteItem(int? id)
        {
            Order = _context.Order.Include(m => m.OrderedItems).ThenInclude(oi => oi.Item).FirstOrDefault(m => m.OrderID == id);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            OrderedItem OrderedItem = _context.OrderedItems.FirstOrDefault(r => (r.OrderID == id) && (r.ItemID == ItemIDToDelete));
            Item ItemBeingDeleted = _context.Item.FirstOrDefault(i => i.ItemID == ItemIDToDelete);

            double NewTotal = Order.OrderTotal - ItemBeingDeleted.Price;
            
            if (OrderedItem != null)
            {
                _context.Remove(OrderedItem);
                _context.SaveChanges();

                var UpdatedOrder = _context.Order.Where(o => o.OrderID == id).ToList();
                UpdatedOrder.ForEach(o => o.OrderTotal=NewTotal);
                _context.SaveChanges();
            }

            Order = _context.Order.Include(m => m.OrderedItems).ThenInclude(oi => oi.Item).FirstOrDefault(m => m.OrderID == id);

            return Page();
        }        
    }
}
