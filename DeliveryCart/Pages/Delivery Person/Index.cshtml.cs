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
    public class IndexModel : PageModel
    {
        private readonly Assignment_2.Models.DatabaseContext _context;

        public IndexModel(Assignment_2.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;
        [BindProperty]
        public int OrderToUpdate {get; set;}

        public async Task OnGetAsync()
        {
            if (_context.Order != null)
            {
                Order = await _context.Order.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string UpdatedStatus)
        {
            Order Order = new Order() { OrderID = id, Status = "Accepted" };
            
            using(_context)
            {
                _context.Order.Attach(Order).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }
             return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeclineOrderAsync(int id, string UpdatedStatus)
        {
            Order Order = new Order() { OrderID = id, Status = "Declined" };
            
            using(_context)
            {
                _context.Order.Attach(Order).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }
             return RedirectToPage("./Index");
        }

    }
}
