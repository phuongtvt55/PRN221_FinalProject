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
    public class OrderManagementModel : PageModel
    {
        private readonly ShoesStoreContext _context;

        public OrderManagementModel()
        {
            _context = new ShoesStoreContext();
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderStatus).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string action ,int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Order order = _context.Orders.SingleOrDefault(o => o.OrderId == id); 
            if (action.Equals("confirm"))
            {
                order.OrderStatusId = 1;
            }
            else
            {
                order.OrderStatusId = 2; 
            }
            _context.Orders.Update(order);  
            _context.SaveChanges();
            return RedirectToPage("./OrderManagement");
        }
    }
}
