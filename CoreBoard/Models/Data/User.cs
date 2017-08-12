using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBoard.Models.Data
{
    public class User
    {
        public long Id { get; set; }
        public string NickName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }


        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
