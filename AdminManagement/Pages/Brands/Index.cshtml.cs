using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;

namespace AdminManagement.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly ShoesStoreContext _context;

        public IndexModel()
        {
            _context = new ShoesStoreContext();
        }

        public IList<Brand> Brand { get;set; }

        public async Task OnGetAsync()
        {
            Brand = await _context.Brands.ToListAsync();
        }
    }
}
