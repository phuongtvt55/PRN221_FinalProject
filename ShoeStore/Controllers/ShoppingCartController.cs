using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShoeStore.Controllers
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class ShoppingCartController : Controller
    {
        ShoesStoreContext _db = new ShoesStoreContext();
    
        public ActionResult AddToCart(int id)
        {
            var quantity = HttpContext.Session.GetInt32("quantity");
            if(quantity == null)
            {
                quantity = 1;
            }
            var spt = _db.Products.FirstOrDefault(s => s.ProductId == id);
            var price = spt.Price;
            List<OrderDetail> cart = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            
            if(cart != null)
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == id && (spt.Quantity - item.Quantity == 0))
                    {
                        TempData["ErrorQuantity"] = "Product is out of stock";
                        return RedirectToAction("Index", "ProductCustomer");
                    }
                }
                var check = cart.FirstOrDefault(s => s.ProductId == id);
                
                if(check == null)
                {
                    OrderDetail sp = new OrderDetail()
                    {
                        ProductId = id,
                        Quantity = quantity,
                        Price = price * quantity,
                        Product = spt
                    };
                    cart.Add(sp);
                }
                else
                {
                    check.Quantity++;
                    check.Price = price * check.Quantity;
                }
                HttpContext.Session.SetObject("cart", cart);
            }
            else
            {
               cart = new List<OrderDetail>();
                OrderDetail sp = new OrderDetail()
                {
                    ProductId = id,
                    Quantity = quantity,
                    Price = price * quantity,
                    Product = spt
                };
                cart.Add(sp);
                HttpContext.Session.SetObject("cart", cart);
            }
            var list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            var sum = 0;
            foreach(var item in list)
            {
                sum += item.Quantity.Value;
            }
            HttpContext.Session.SetInt32("bag", sum);
            HttpContext.Session.Remove("quantity");
            return RedirectToAction("Index", "ProductCustomer");
        }

        public ActionResult ViewCart()
        {
            List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            if(list != null)
            {
                List<OrderDetail> list1 = new List<OrderDetail>();
                var sum = 0;
                foreach (var d in list)
                {
                    sum += Convert.ToInt32(d.Price);
                    list1.Add(d);
                }
                ViewBag.TotalPrice = sum;
                return View(list1);
            }
            else
            {
                return View(list);
            }
            
        }

        public ActionResult ViewOrder()
        {
            var cart = new List<OrderDetail>();
            var cusId = HttpContext.Session.GetInt32("CustomerId");
            if(cusId == null)
            {
                TempData["Error"] = "You must to login first to see order";
                return RedirectToAction("Index", "Login");
            }
            var role = HttpContext.Session.GetString("Role");
            try
            {
                using var context = new ShoesStoreContext();
                if(role == "Customer")
                {
                    IAccountRepository accountRepository = new AccountRepository();
                    var account = accountRepository.GetAccounts();
                    ViewBag.Email = account;
                    cart = context.OrderDetails.Include(b => b.Product).Include(o => o.Order).Where(c => c.Order.CustomerId == cusId).ToList();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return View(cart);
        }

        public ActionResult Update_Quantity_Cart(IFormCollection form)
        {
            List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
           
            var status = form["status"];
            var proid = int.Parse(form["proId"]);
            var spt = _db.Products.FirstOrDefault(s => s.ProductId == proid);
            var quantity = int.Parse(form["quantity"]);
            var price = spt.Price;
           
            if(list != null)
            {
                int count = 0;
                foreach (var item in list)
                {
                    if (item.ProductId == proid)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    HttpContext.Session.SetInt32("quantity", quantity);
                    AddToCart(proid);
                    return RedirectToAction("Index", "ProductCustomer");
                }
            }
            else
            {
                HttpContext.Session.SetInt32("quantity", quantity);
                AddToCart(proid);                
                return RedirectToAction("Index", "ProductCustomer");
            }
            
            var check = list.FirstOrDefault(s => s.ProductId == proid);
            if (status.Equals("detail"))
            {
                if(check == null)
                {
                    check.Quantity = quantity;
                }
                else
                {
                    check.Quantity += quantity;
                }
            }
            else
            {
                check.Quantity = quantity;
            }
            check.Price = check.Quantity * price;
            HttpContext.Session.SetObject("cart", list);
            var list1 = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            var sum = 0;
            foreach (var item in list1)
            {
                sum += item.Quantity.Value;
            }
            HttpContext.Session.SetInt32("bag", sum);
          
            return RedirectToAction("ViewCart");
        }

        public ActionResult Remove(int id)
        {
            List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            var check = list.FirstOrDefault(s => s.ProductId == id);
            list.Remove(check);
            HttpContext.Session.SetObject("cart", list);
            var list1 = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            var sum = 0;
            foreach (var item in list1)
            {
                sum += item.Quantity.Value;
            }
            HttpContext.Session.SetInt32("bag", sum);
            return RedirectToAction("ViewCart");
        }

        public ActionResult CheckOut()
        {
            var check = HttpContext.Session.GetString("Role");
            if(check != null)
            {
                List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
                if (list != null)
                {
                    List<OrderDetail> list1 = new List<OrderDetail>();
                    var sum = 0;
                    foreach (var d in list)
                    {
                        sum += Convert.ToInt32(d.Price);
                        list1.Add(d);
                    }
                    ViewBag.TotalPrice = sum;
                    return View(list1);
                }
                else
                {
                    return View(list);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Order(IFormCollection form)
        {
            var cusId = HttpContext.Session.GetInt32("CustomerId");
            var phone = int.Parse(form["phone"]);
            var address = form["address"];
            var total = decimal.Parse(form["totalPrice"]);
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.Address = address;
            order.Phone = phone;
            order.CustomerId = cusId.Value;
            order.TotalPrice = total;
            order.OrderStatusId = 1;
            List<OrderDetail> list = HttpContext.Session.GetObject<List<OrderDetail>>("cart");
            if(list.Count == 0)
            {
                TempData["ErrorMessage"] = "Please order some product";
                return RedirectToAction("Index", "ProductCustomer");
            }
            _db.Orders.Add(order);
            _db.SaveChanges();

            
            foreach(var item in list)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.OrderId;
                orderDetail.ProductId = item.ProductId;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Price = item.Price;
                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();
                using var context = new ShoesStoreContext();
                Product product = null;
                product = context.Products.SingleOrDefault(s => s.ProductId == item.ProductId);
                product.Quantity -= item.Quantity;
                context.Products.Update(product);
                context.SaveChanges();
            }
            list.Clear();
            HttpContext.Session.SetObject("cart", list);
            HttpContext.Session.Remove("bag");
            return RedirectToAction("Index", "ProductCustomer");
        }
    }
}
