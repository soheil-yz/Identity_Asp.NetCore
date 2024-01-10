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
        private readonly SignInManager<Users> _signInManager;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        #region IActionResult Register
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
        #endregion

        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginDto
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var Users = _userManager.FindByNameAsync(loginDto.UserName).Result;

            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync
                (Users, loginDto.Password, loginDto.IsPersistent, true).Result;
            //اگر از await 
            //استفاده بشه دیگر نیاز نیست .Result
            if (result.Succeeded)
                return Redirect(loginDto.ReturnUrl);

            if (result.RequiresTwoFactor)
            {

            }
            if (result.IsLockedOut)
            {

            }
            ModelState.AddModelError(string.Empty, "You Could Not Log in !!!");

            return View();
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
 




