using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreBoard.Models.Data
{
    public class Comment
    {
        public long Id { get; set; }
        
        [DisplayName("내용")]
        public string Content { get; set; }

        public long? WriterId { get; set; }
        public User Writer { get; set; }
        public string WriterName { get; set; }

        public string Password { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
        public long PostId { get; set; }
    }
}
