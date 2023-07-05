using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShoeStore.Controllers
{
    public class LoginController : Controller
    {
        IAccountRepository accountRepository = null;
        public LoginController()
        {
            accountRepository = new AccountRepository();
        }
        public IActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("CustomerId");
            return View();
        }

        public IActionResult Login()
        {
            var name = Request.Form["email"];
            var pass = Request.Form["password"];
            var account = accountRepository.GetAccounts();
            foreach (var accountItem in account)
            {
                if (name == accountItem.Email && pass == accountItem.Password)
                {
                    HttpContext.Session.SetString("Role", "Customer");
                    HttpContext.Session.SetInt32("CustomerId", accountItem.CustomerId);
                    return RedirectToAction("Index", "ProductCustomer");
                }
               
            }
            ViewBag.Error = "Wrong Email or Password";
            return View("Index");
        }


        public IActionResult SignUp()
        {
            var name = Request.Form["name"];
            var email = Request.Form["email"];
            var pass = Request.Form["password"];
            var confirm = Request.Form["confirm"];
            var account = accountRepository.GetAccountByEmail(name);
            if(account == null)
            {
                if (pass == confirm)
                {
                    Account a = new Account();
                    a.Email = email;
                    a.Password = pass;
                    a.Role = 0;
                    a.Name = name;
                    var _db = new ShoesStoreContext();
                    _db.Accounts.Add(a);
                    _db.SaveChanges();
                    HttpContext.Session.SetString("Role", "Customer");
                    HttpContext.Session.SetInt32("CustomerId", a.CustomerId);
                    return RedirectToAction("Index", "ProductCustomer");
                }
                else
                {
                    ViewBag.Error = "Confirm password incorect";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.Error = "Email is already in use";
                return View("Index");
            }
        }
    }
}
