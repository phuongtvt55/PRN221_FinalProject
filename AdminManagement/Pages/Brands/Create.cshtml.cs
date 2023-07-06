using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AdminManagement.Pages.Brands
{
    public class CreateModel : PageModel
    {
        private readonly ShoesStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(IWebHostEnvironment webHostEnvironment)
        {
            _context = new ShoesStoreContext();
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Brand Brand { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            
            if(image == null || image.Length == 0)
            {
                return NotFound();
            }

            Brand.LogoUrl = await UploadImage(image);
            Brand.Status = 1;
            _context.Brands.Add(Brand);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return "ErrorImg";

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            string path2 = path.Replace("AdminManagement", "ShoeStore");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            using (var stream = new FileStream(path2, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "images/" + fileName;
        }
    }
}
