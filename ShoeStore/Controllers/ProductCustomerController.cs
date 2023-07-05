using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ShoeStore.Controllers
{
    public class ProductCustomerController : Controller
    {
        IProductRepositoty productRepositoty = null;
        public ProductCustomerController()
        {
            productRepositoty = new ProductRepository();
        }
        // GET: ProductController
        public ActionResult Index()
        {
            IBrandRepository brandRepository = new BrandRepository();
            var brandList = brandRepository.GetBrands();
            ViewBag.BrandList = brandList;
            var list = productRepositoty.GetProducts();
            ViewBag.Error = TempData["ErrorQuantity"] as string;
            return View(list);
        }

        // GET: ProductController/ViewBrand/1
        public ActionResult ViewBrand(int id)
        {
            IBrandRepository brandRepository = new BrandRepository();
            var brandList = brandRepository.GetBrands();
            ViewBag.BrandList = brandList;
            var list = productRepositoty.GetProductByBrand(id);
            return View("Index", list);
        }
        // GET: ProductController/Search/name
        public ActionResult Search()
        {
            IBrandRepository brandRepository = new BrandRepository();
            var brandList = brandRepository.GetBrands();
            ViewBag.BrandList = brandList;
            var name = Request.Form["name"];
            var list = productRepositoty.Search(name);
            return View("Index", list);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var listProduct = productRepositoty.GetProducts();
            ViewBag.ProductList = listProduct;
            var product = productRepositoty.GetProductById(id);
            List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            ViewBag.Value = 1;
            int newQuantity = product.Quantity.Value;
            if (product == null)
            {
                return NotFound();
            }
            if(list != null)
            {
                foreach (var item in list)
                {
                    if (item.ProductId == product.ProductId)
                    {
                        newQuantity = product.Quantity.Value - item.Quantity.Value;
                        if(newQuantity <= 0)
                        {
                            newQuantity = 0;
                            ViewBag.Value = 0;
                        }
                    }
                }
            }
            
            ViewBag.Quantity = newQuantity;
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
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.ImageUrl = "images/" + product.ImageUrl;
                    productRepositoty.Insert(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
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
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    product.ImageUrl = "images/" + product.ImageUrl;
                    productRepositoty.Update(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = productRepositoty.GetProductById(id);
            if (product == null)
            {
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
        [HttpPost]
        public ActionResult Cart(IFormCollection form)
        {
            var productid = int.Parse(form["productId"]);
            var quantity = int.Parse(form["quantity"]);
            return View();
        }
    }
}
