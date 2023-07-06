using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int OrderStatusId { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }

        public virtual Account Customer { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
