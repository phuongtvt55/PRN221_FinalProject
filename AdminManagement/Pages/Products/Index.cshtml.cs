using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess.Repository;

namespace AdminManagement.Pages.Products
{
    public class IndexModel : PageModel
    {
        IProductRepositoty productRepositoty = null;
        //private readonly BusinessObject.ShoesStoreContext _context;

        public IndexModel()
        {
            //_context = context;
            productRepositoty = new ProductRepository();
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            var dbContext = new ShoesStoreContext();
            var result = dbContext.Orders
                        .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                        .Select(g => new
                        {
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            TotalPrice = g.Sum(o => o.TotalPrice)
                        })
                        .ToList();

            var currentYear = DateTime.Now.Year;

            var result1 = dbContext.Orders
                .Where(o => o.OrderDate.Value.Year == currentYear)
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalPrice = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(g => g.Month)
                .ToList();
            result.ForEach(o =>
            {
                
            });
            Product = (IList<Product>)productRepositoty.GetProductListAdmin();
        }
    }
}
