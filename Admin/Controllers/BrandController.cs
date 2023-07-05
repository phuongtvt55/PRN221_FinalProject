using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using DataAccess.Repository;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ShoeStore.Controllers
{
    public class BrandController : Controller
    {
        IBrandRepository repository = null;
        public BrandController()
        {
            repository = new BrandRepository();
        }
        // GET: BrandController
        public ActionResult Index()
        {
            
            using var context = new ShoesStoreContext();
            var list = context.Brands.ToList();
            return View(list);
        }

        // GET: BrandController/Details/5
        public ActionResult Details(int id)
        {
            var list = repository.GetBrandById(id);
            if(list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        // GET: BrandController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand brand, IFormFile image)
        {
            try
            {
               
                    brand.LogoUrl = await UploadImage(image);
                    repository.Insert(brand);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: BrandController/Edit/5
        public ActionResult Edit(int id)
        {
            Brand brand = repository.GetBrandById(id);
            if(brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand brand, IFormFile image)
        {
            try
            {   if(id != brand.BrandId)
                {
                    return NotFound();
                }
                
                    brand.LogoUrl = await UploadImage(image);
                    repository.Update(brand);
                
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

        // GET: BrandController/Delete/5
        public ActionResult Delete(int id)
        {
            var list = repository.GetBrandById(id);
            return View(list);
        }

        // POST: BrandController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Brand brand)
        {
            try
            {
                brand = repository.GetBrandById(id);
                brand.Status = 0;
                repository.Update(brand);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
