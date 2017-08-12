using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreBoard.Helpers;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public class AuthorizationService
    {
        private readonly DatabaseContext _database;

        public AuthorizationService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<User> AuthorizeAsync(string id, string password)
        {
            var user = await _database.Users.SingleOrDefaultAsync(u => u.Email == id);
            if (user == null)
                return null;

            return Crypto.VerifyHashedPassword(user.Password, password) ? user : null;
        }

        public async Task<User> CreateAsync(User source, string id, string password)
        {
            source.Email = id;
            source.Password = Crypto.HashPassword(password);

            _database.Users.Add(source);
            await _database.SaveChangesAsync();

            return source;
        }
    }
}
