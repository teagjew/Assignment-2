using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_2.Models;

namespace Assignment_2.Pages.DeliveryPerson
{
    public class EditModel : PageModel
    {
        private readonly Assignment_2.Models.DatabaseContext _context;

        public EditModel(Assignment_2.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public int OrderToUpdate {get; set;}
        [BindProperty]
        public string UpdatedStatus {get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Order.FirstOrDefaultAsync(p => p.OrderID == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.OrderID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string UpdatedStatus)
        {
            Order Order = new Order() { OrderID = id, Status = UpdatedStatus };
            
            using(_context)
            {
                _context.Order.Attach(Order).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }
            return RedirectToPage("./Index");
        }

        

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
