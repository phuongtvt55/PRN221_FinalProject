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
using DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;

namespace AdminManagement.Pages.Products
{

    public class CreateModel : PageModel
    {
        IProductRepositoty productRepositoty = null;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CreateModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            productRepositoty = new ProductRepository();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return NotFound();


                Product.ImageUrl = await UploadImage(image);
                Product.Status = 1;
                productRepositoty.Insert(Product);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
               
                return NotFound();
            }
            
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
