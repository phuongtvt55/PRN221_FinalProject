using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Account
    {
        public Account()
        {
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? Role { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
