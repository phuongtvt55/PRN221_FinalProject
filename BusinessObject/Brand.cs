using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Input can not be null!")]
        public string Name { get; set; }
        [DisplayName("Image")]
        [Required(ErrorMessage = "Input can not be null!")]
        public string LogoUrl { get; set; }
        [Required(ErrorMessage = "Input can not be null!")]
        [Range(0, 1, ErrorMessage = "Please input 1 or 0")]
        public int? Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
