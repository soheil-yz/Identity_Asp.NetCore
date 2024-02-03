using Azure.Identity;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager = null, EmailService emailService = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callBackUrl = Url.Action
                    ("ConfirmEmail", "Account",
                    new { UserId = newUser.Id, token = token },
                    protocol: Request.Scheme);

                string body = $"Pleas Click On This  <br /> <a href={callBackUrl}>Link !</a>  For Enable Your Portfo";
                _emailService.Execute(newUser.Email, body, "Enable your Portfo");

                return RedirectToAction("DisplayEmail");
            }

            string message = "";
            foreach (var item in Users.Errors.ToList())
                message += item.Description + Environment.NewLine;

            TempData["Massege"] = message;
            return View(registerDto);
        }
        #endregion

        public IActionResult ConfirmEmail(string UserId , string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
                return View("Error");
            var result = _userManager.ConfirmEmailAsync(user, Token).Result;
            if (result.Succeeded)
            {

            }

            return RedirectToAction("login");
        }
        public IActionResult DisplayEmail()
        {
            return View();
        }
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
            if (Users == null)
            {
                TempData["Register"] = "First you have to Register";
                return RedirectToAction("Register", "Account");
            }
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
            ModelState.AddModelError(string.Empty, "Your Password is Wroing !!!");

            return View();
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}





