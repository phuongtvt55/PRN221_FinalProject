using Admin.Models;
using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ViewOrder()
        {
            var cart = new List<OrderDetail>();
            var order = new List<Order>();
            using var context = new ShoesStoreContext();

            IAccountRepository accountRepository = new AccountRepository();
            var account = accountRepository.GetAccounts();
            ViewBag.Email = account;
            cart = context.OrderDetails.Include(b => b.Product).Include(o => o.Order).ToList();
            order = context.Orders.Include(c => c.Customer).Include(s => s.OrderStatus).ToList();
            ViewBag.Order = order;
            return View(cart);
        }
    }
}
