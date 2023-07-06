using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace AdminManagement.Pages.Brands
{
    public class DetailsModel : PageModel
    {
        private readonly ShoesStoreContext _context;

        public DetailsModel()
        {
            _context = new ShoesStoreContext();
        }

        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Brand = await _context.Brands.FirstOrDefaultAsync(m => m.BrandId == id);

            if (Brand == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
