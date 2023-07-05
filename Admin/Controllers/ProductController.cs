using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using System;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShoeStore.Controllers
{
    public class ProductController : Controller
    {
        IProductRepositoty productRepositoty = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            productRepositoty = new ProductRepository();
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var list = productRepositoty.GetProductListAdmin();
            return View(list);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = productRepositoty.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            IBrandRepository brandRepository = new BrandRepository();
            var brand = brandRepository.GetBrands();
            ViewBag.BrandList = brand;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product product, IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return View("Index");
                
               
                product.ImageUrl = await UploadImage(image);
                productRepositoty.Insert(product);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }


        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            IBrandRepository brandRepository = new BrandRepository();
            var brand = brandRepository.GetBrands();
            ViewBag.BrandList = brand;
            var product = productRepositoty.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Product product, IFormFile image)
        {
            try
            {
                if(id != product.ProductId)
                {
                    return NotFound();
                }
                
                product.ImageUrl = await UploadImage(image);

                    productRepositoty.Update(product);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return "ErrorImg";

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            string path2 = path.Replace("Admin", "ShoeStore");

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

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = productRepositoty.GetProductById(id);
            if(product == null) {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                product = productRepositoty.GetProductById(id);
                product.Status = 0;
                productRepositoty.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
