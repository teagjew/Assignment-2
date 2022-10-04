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
            Items = await _context.ListItems();
        }

        public async Task<IActionResult> OnPostAddItemAsync(int id)
        {
            await _context.AddToCart(id);

            return RedirectToPage("./Index");
        }
    }
}
