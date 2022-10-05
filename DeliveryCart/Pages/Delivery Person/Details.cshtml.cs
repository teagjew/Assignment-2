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

        public async Task OnGet(int id)
        {
            Order = await _context.OrderDetails(id);
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int id)
        {
            Order = await _context.OrderDetails(id);

            await _context.RemoveOrderItem(id, ItemIDToDelete, Order.OrderTotal);

            Order = await _context.OrderDetails(id);

            return Page();
        }        
    }
}
