using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment_2.Models;

namespace Assignment_2.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly Assignment_2.Models.OrderContext _context;

        public DetailsModel(Assignment_2.Models.OrderContext context)
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

            Order = await _context.Order.Include(m => m.Items).FirstOrDefaultAsync(m => m.OrderID == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostDeleteItem(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Item Item = _context.Item.FirstOrDefault(r => r.ItemID == ItemIDToDelete);
            
            if (Item != null)
            {
                _context.Remove(Item);
                _context.SaveChanges();
            }

            Order = _context.Order.Include(m => m.Items).FirstOrDefault(m => m.OrderID == id);

            return Page();
        }        
    }
}
