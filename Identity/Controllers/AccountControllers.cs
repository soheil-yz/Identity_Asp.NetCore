using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountControllers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
