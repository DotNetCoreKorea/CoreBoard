using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreBoard.Services;

namespace CoreBoard.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthorizationService _authService;
        private readonly ISessionService _session;

        public AuthController(AuthorizationService authService, ISessionService session)
        {
            _authService = authService;
            _session = session;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = "/")
        {
            var user = await _authService.AuthorizeAsync(email, password);
            if (user == null)
                return View();

            await _session.LoginAsync(user);

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _session.LogoutAsync();
            return Redirect(returnUrl);
        }
    }
}
