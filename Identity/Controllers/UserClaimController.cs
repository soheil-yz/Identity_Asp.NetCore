using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class UserClaimController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View(User.Claims);
        }
    }
}
