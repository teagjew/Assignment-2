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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.OrderDetails(id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string UpdatedStatus)
        {
            await _context.UpdateStatus(id, UpdatedStatus);

            return RedirectToPage("./Index");
        }
    }
}
