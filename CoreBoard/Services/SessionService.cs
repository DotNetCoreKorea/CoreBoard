using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CoreBoard.Extensions;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public class SessionService:ISessionService
    {
        private const string SessionKey = "_account";

        private readonly HttpContext _context;
        private readonly DatabaseContext _database;

        public SessionService(IHttpContextAccessor contextAccessor, DatabaseContext database)
        {
            _database = database;
            _context = contextAccessor.HttpContext;
        }

        public async Task<User> GetUserAsync()
        {
            var user = _context.Items[SessionKey] as User;
            if (user == null)
            {
                var id = _context.Session.GetInt64(SessionKey);
                if (id == null)
                    return null;

                user = await _database.Users.FindAsync(id);

                _context.Items[SessionKey] = user;
            }

            return user;
        }

        public async Task LoginAsync(User user)
        {
            _context.Items[SessionKey] = user;
            _context.Session.SetInt64(SessionKey, user.Id);

            user.UpdatedAt = DateTimeOffset.Now;
            await _database.SaveChangesAsync();
        }

        public async Task LogoutAsync()
        {
            _context.Items[SessionKey] = null;
            _context.Session.Remove(SessionKey);
        }

        public bool IsLoggedIn => GetUserAsync().Result != null;
    }
}
