using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBoard.Models.Request
{
    public class PostPostModel
    {
        [DisplayName("제목")]
        [Required(ErrorMessage = "제목을 입력하세요")]
        public string Title { get; set; }

        [DisplayName("내용")]
        [Required(ErrorMessage = "내용을 입력하세요")]
        public string Content { get; set; }

        [DisplayName("비밀번호")]
        public string Password { get; set; }

        [DisplayName("작성자 이름")]
        public string WriterName { get; set; }

        public string AnonymousToken { get; set; }
    }
}
