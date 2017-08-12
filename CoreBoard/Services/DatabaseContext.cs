using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
