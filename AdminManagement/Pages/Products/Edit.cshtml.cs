using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using System.IO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;

namespace AdminManagement.Pages.Products
{
    public class EditModel : PageModel
    {
        IProductRepositoty productRepositoty = null;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public EditModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            productRepositoty = new ProductRepository();
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product = productRepositoty.GetProductById(id);


            if (Product == null)
            {
                return NotFound();
            }
            var _context = new ShoesStoreContext();
           ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id, IFormFile image)
        {
            try
            {
                var number = Request.Form["number"];
                if (id != Product.ProductId)
                {
                    return NotFound();
                }

                Product.ImageUrl = await UploadImage(image);
                Product.Status = Convert.ToInt32(number);
                productRepositoty.Update(Product);

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
