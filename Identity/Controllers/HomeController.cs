using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Policy = "Buyer1")]
        public string JustBuyer()
        {
            return "Are You Buyer";
        }

        [Authorize(Policy = "BloodType")]
        public string Blood()
        {
            return "Ap Or O";
        }

        [Authorize(Policy = "Credit")]
        public string Credit()
        {
            return "Credit";
        }
    }
}