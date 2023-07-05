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
            Product = (IList<Product>)productRepositoty.GetProductListAdmin();
        }
    }
}
