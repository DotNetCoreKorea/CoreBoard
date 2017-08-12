using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBoard.Models.Data;

namespace CoreBoard.Services
{
    public interface ISessionService
    {
        Task<User> GetUserAsync();
        Task LoginAsync(User user);
        Task LogoutAsync();

        bool IsLoggedIn { get; }
    }
}
