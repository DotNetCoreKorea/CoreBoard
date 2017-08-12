using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreBoard.Helpers;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public class PostService : IPostService
    {
        private readonly DatabaseContext _database;

        public PostService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<Post> CreatePostAsync(User writer, string title, string content, string password = null, string writerName = null)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                Password = Crypto.HashPassword(password ?? ""),
                Writer = writer,
                WriterName = writer?.NickName ?? writerName
            };

            _database.Posts.Add(post);
            await _database.SaveChangesAsync();

            return post;
        }

        public async Task<Post[]> ListPostAsync(int take = 10, int page = 0)
        {
            var posts = await _database.Posts.Include(p => p.Writer)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(take * page)
                .Take(take)
                .ToArrayAsync();

            return posts;
        }

        public async Task<Post> GetPostAsync(long id)
        {
            var post = await _database.Posts.Include(p => p.Writer).SingleOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<Post> UpdatePostAsync(long postId, string title, string content, string password)
        {
            var post = await _database.Posts.FindAsync(postId);

            post.Title = title;
            post.Content = content;
            post.Password = Crypto.HashPassword(password ?? "");

            await _database.SaveChangesAsync();

            return post;
        }

        public async Task DeletePostAsync(long postId)
        {
            var post = await _database.Posts.FindAsync(postId);

            _database.Posts.Remove(post);
            await _database.SaveChangesAsync();
        }
    }
}
