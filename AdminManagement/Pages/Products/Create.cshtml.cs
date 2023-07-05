using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;

namespace AdminManagement.Pages.Products
{
    public class CreateModel : PageModel
    {
       

        public CreateModel()
        {
        
        }

        public IActionResult OnGet()
        {
            var _context = new ShoesStoreContext();
        ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var _context = new ShoesStoreContext();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
