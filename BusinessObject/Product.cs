using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Product
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
        public int? Quantity { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
