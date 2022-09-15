using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment_2.Models;

namespace Assignment_2.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly Assignment_2.Models.DatabaseContext _context;

        public IndexModel(Assignment_2.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Item> Items { get; set; }

        [BindProperty]
        public int ItemToAdd { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Item != null)
            {
                Items = await _context.Item.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAddItemAsync(int id)
        {
            Item item = new Item() { ItemID = id, Status = "In Cart" };
            
            using(_context)
            {
                _context.Item.Attach(item).Property(i => i.Status).IsModified = true;
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");

        }
    }
}
