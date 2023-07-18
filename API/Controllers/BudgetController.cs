using API.Model;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    
    public class BudgetController : Controller
    {
        private readonly ShoesStoreContext _db;

        public BudgetController()
        {
            this._db = new ShoesStoreContext();
        }

        [HttpGet]
        [Route("GetBudGet")]
        public List<BudgetByMonth> GetBudGet()
        {
            var currentYear = DateTime.Now.Year;
            var budget = this._db.Orders
                .Where(o => o.OrderDate.Value.Year == currentYear)
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new BudgetByMonth
                {
                    Year = g.Key.Year.ToString(),
                    Month = g.Key.Month.ToString(),
                    TotalPrice = g.Sum(o => o.TotalPrice).ToString()
                })
                .OrderBy(g => g.Month)
                .ToList();

            return budget;
        }
    }
}
