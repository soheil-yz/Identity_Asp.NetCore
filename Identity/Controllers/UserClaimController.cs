using Identity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Controllers
{
    public class UserClaimController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public UserClaimController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(User.Claims);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string ClaimType , string ClaimValue)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Claim claim = new Claim(ClaimType, ClaimValue, ClaimValueTypes.String);
            var result = _userManager.AddClaimAsync(user, claim).Result;

            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var i in result.Errors) 
                {
                    ModelState.AddModelError("", i.Description);
                }
            }
            return View();
        }
    }
}
