using Azure.Identity;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public AccountController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(registerDto);
            }
            Users newUser = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email

            };

            var Users = _userManager.CreateAsync(newUser, registerDto.Password).Result;
            if (Users.Succeeded)
                return RedirectToAction("Index","Home");

            string message ="";
            foreach (var item in Users.Errors.ToList())
                message += item.Description + Environment.NewLine;

            TempData["Massege"] = message;
            return View(registerDto);
        }
    }
}
 




