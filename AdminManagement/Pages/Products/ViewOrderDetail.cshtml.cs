using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace AdminManagement.Pages.Products
{
    public class ViewOrderDetailModel : PageModel
    {
        private readonly ShoesStoreContext _context;

        public ViewOrderDetailModel()
        {
            _context = new ShoesStoreContext();
        }

        public IList<OrderDetail> OrderDetail { get;set; }

        public async Task OnGetAsync(int? id)
        {
            OrderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product).Where(c => c.OrderId == id).ToListAsync();
        }
    }
}
