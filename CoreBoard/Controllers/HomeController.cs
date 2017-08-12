using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreBoard.Services;
using CoreBoard.Models.Data;

namespace CoreBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _database;

        public HomeController(DatabaseContext database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
