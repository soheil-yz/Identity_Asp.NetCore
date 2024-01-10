using Identity.Models.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register(RegisterDto registerDto)
        {
            return View();
        }
    }
}
