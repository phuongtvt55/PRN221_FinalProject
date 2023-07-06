using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
