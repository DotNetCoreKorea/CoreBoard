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
            var post = await _database.Posts
                .Include(p => p.Writer)
                .Include(p => p.Comments)
                .SingleOrDefaultAsync(p => p.Id == id);

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
            var post = await _database.Posts
                .Include(p => p.Comments)
                .SingleOrDefaultAsync(p => p.Id == postId);

            _database.Comments.RemoveRange(post.Comments);
            _database.Posts.Remove(post);

            await _database.SaveChangesAsync();
        }


        public async Task<Comment> CreateCommentAsync(Post post, User writer, string content, string password = null, string writerName = null)
        {
            if (post.Comments == null)
                await _database.Entry(post).Collection(p => p.Comments).LoadAsync();

            var comment = new Comment
            {
                Content = content,
                Password = Crypto.HashPassword(password ?? ""),
                Writer = writer,
                WriterName = writer?.NickName ?? writerName
            };

            post.Comments.Add(comment);

            await _database.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(long commentId, string content, string password)
        {
            var comment = await _database.Comments.FindAsync(commentId);
            
            comment.Content = content;
            comment.Password = Crypto.HashPassword(password ?? "");

            await _database.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentAsync(long commentId)
        {
            var comment = await _database.Comments.FindAsync(commentId);

            _database.Comments.Remove(comment);

            await _database.SaveChangesAsync();
        }
    }
}
