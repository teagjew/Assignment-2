using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Assignment_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment_2.Pages
{
    public class AddItemModel : PageModel
    {
        private readonly ILogger<AddItemModel> _logger;
        private readonly OrderContext _context;
        [BindProperty]
        public Item Item {get; set;}
        public SelectList ItemDropDown {get; set;}

        public AddItemModel(OrderContext context, ILogger<AddItemModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet()
        {
            ItemDropDown = new SelectList(_context.Item.ToList(), "OrderID", "Name");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Item.Add(Item);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}