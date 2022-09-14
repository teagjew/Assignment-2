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
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;

        private readonly Assignment_2.Models.DatabaseContext _context;

        public DetailsModel(Assignment_2.Models.DatabaseContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Item Item { get; set; }

        [BindProperty]
        public int ItemToAdd { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Item = await _context.Item.FirstOrDefaultAsync(i => i.ItemID == id);

            if (Item == null)
            {
                return NotFound();
            }
            return Page();
        }    
    }
}
