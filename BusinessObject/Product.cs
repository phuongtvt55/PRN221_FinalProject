using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
        [Required(ErrorMessage = "Input can not be null!")]
        [DisplayName("Brand Name")]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        public string Description { get; set; }
        [DisplayName("Image")]
        [Required(ErrorMessage = "Input can not be null!")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Range(1, int.MaxValue, ErrorMessage = "Onlly positive number")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        [Range(0, 1, ErrorMessage = "Please input 1 or 0")]
        public int? Status { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        [Range(1, int.MaxValue, ErrorMessage = "Onlly positive number")]
        public int? Quantity { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
