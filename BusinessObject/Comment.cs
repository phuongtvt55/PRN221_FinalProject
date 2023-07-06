using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string CommentMsg { get; set; }
        public DateTime? CommentDate { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public long? ParentId { get; set; }
        public int? Rate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Account User { get; set; }
    }
}
