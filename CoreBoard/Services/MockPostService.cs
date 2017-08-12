using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBoard.Helpers;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public class MockPostService : IPostService
    {
        private static HashSet<Post> Posts { get; set; } = new HashSet<Post>();

        public async Task<Post> CreatePostAsync(User writer, string title, string content, string password = null, string writerName = null)
        {
            var post = new Post
            {
                Id = Posts.Count + 1,
                Title = title,
                Content = content,
                Password = Crypto.HashPassword(password??""),
                Writer = writer
            };

            Posts.Add(post);
            
            return post;
        }

        public async Task<Post[]> ListPostAsync(int take = 10, int page = 0)
        {
            return Posts.Skip(take * page).Take(take).ToArray();
        }

        public async Task<Post> GetPostAsync(long id)
        {
            return Posts.SingleOrDefault(p => p.Id == id);
        }

        public async Task<Post> UpdatePostAsync(long postId, string title, string content, string password)
        {
            var post = Posts.SingleOrDefault(p => p.Id == postId);

            if (post == null)
                return null;

            if (Crypto.VerifyHashedPassword(post.Password, password))
                throw new UnauthorizedAccessException();

            post.Title = (String.IsNullOrWhiteSpace(title) ? post.Title : title);
            post.Content = (String.IsNullOrWhiteSpace(content) ? post.Title : title);

            return post;
        }

        public async Task DeletePostAsync(long postId)
        {
            var post = Posts.SingleOrDefault(p => p.Id == postId);

            if (post == null)
                return;

            Posts.Remove(post);
        }
    }
}
