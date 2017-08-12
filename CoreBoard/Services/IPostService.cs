using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(User writer, string title, string content, string password = null, string writerName = null);
        Task<Post[]> ListPostAsync(int take = 10, int page = 0);
        Task<Post> GetPostAsync(long id);
        Task<Post> UpdatePostAsync(long postId, string title, string content, string password);
        Task DeletePostAsync(long postId);
    }
}
