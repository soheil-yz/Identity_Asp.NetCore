using Azure.Identity;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto;
using Identity.Models.Entities.Dto.Accounts;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using System.Drawing.Text;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, EmailService emailService = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            MyAccountInfoDto accountInfo = new MyAccountInfoDto()
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FullName = user.FirstName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                PhoneConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            };
            return View(accountInfo);
        }
        [Authorize]
        public IActionResult TwoFactoryEnable()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = _userManager.SetTwoFactorEnabledAsync(user,!user.TwoFactorEnabled).Result;
            return RedirectToAction(nameof(Index));
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
                    ("ConfirmEmail", "Account", new
                    {
                        UserId = newUser.Id
                    ,
                        token = token
                    }, protocol: Request.Scheme);

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

        public IActionResult ConfirmEmail(string UserId, string Token)
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

            if (result.RequiresTwoFactor == true)
            {
                return RedirectToAction("TwoFactorLogin", new { loginDto.UserName, loginDto.IsPersistent});
            }
            if (result.IsLockedOut)
            {

            }
            ModelState.AddModelError(string.Empty, "Your Password is Wroing !!!");

            return View();
        }
        public IActionResult TwoFactorLogin(string UserName, bool IsPersistent)
        {
            var user = _userManager.FindByNameAsync (UserName).Result;
            if(user == null)
            {
                return BadRequest();
            }
            var providers = _userManager.GetValidTwoFactorProvidersAsync(user).Result;
            if (providers.Contains("Email"))
            {
                string EmailCode = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;
                EmailService emailService = new EmailService();
                emailService.Execute(user.Email, $"Two Factory Code:{EmailCode} ","Two Factory Login");

            }
            else if (providers.Contains("Phone"))
            {
                string smsCode = _userManager.GenerateTwoFactorTokenAsync(user , "Phone").Result;
                SMSService sms = new SMSService();
                sms.Send(user.PhoneNumber , smsCode);
            }
            return View();
        }


        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordConfirmationDto forgotPassword)
        {
            if (!ModelState.IsValid)
                return View(forgotPassword);

            var user = _userManager.FindByEmailAsync(forgotPassword.Email).Result;
            if (user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                ViewBag.meesage = "ممکن است ایمیل وارد شده معتبر نباشد! و یا اینکه ایمیل خود را تایید نکرده باشید";
                return View();
            }
            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            string callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                UserId = user.Id,
                token = token
            },
            protocol: Request.Scheme);

            string body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید <br/> <a href={callbackUrl}> link reset Password </a>";
            _emailService.Execute(user.Email, body, "فراموشی رمز عبور");
            ViewBag.meesage = "لینک تنظیم مجدد کلمه عبور برای ایمیل شما ارسال شد";
            return View();
        }

        public IActionResult ResetPassword(string UserId, string Token)
        {

            return View(new ResetPasswordDto
            {
                Token = Token,
                UserId = UserId
            });
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto passwordDto)
        {
            if (!ModelState.IsValid)
                return View(passwordDto);
            if (passwordDto.ConfirmPassword != passwordDto.Password)
                return BadRequest();

            var user = _userManager.FindByIdAsync(passwordDto.UserId).Result;
            if (user == null)
                return BadRequest();

            var Result = _userManager.ResetPasswordAsync(user, passwordDto.Token, passwordDto.Password).Result;

            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            else
            {
                ViewBag.Errors = Result.Errors;
                return View(passwordDto);
            }
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public IActionResult SetPhoneNumber()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult SetPhoneNumber(SetPhoneNumberDto numberDto)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var setResult = _userManager.SetPhoneNumberAsync(user, numberDto.PhoneNumber).Result;
            string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, numberDto.PhoneNumber).Result;

            SMSService smsService = new SMSService();
            smsService.Send(numberDto.PhoneNumber, code);
            TempData["phone"] = numberDto.PhoneNumber;

            return RedirectToAction(nameof(VerifyPhoneNumber));
        }
        public IActionResult VerifyPhoneNumber()
        {
            return View(new VerifyPhoneNumberDto
            {
                PhoneNumber = TempData["Phone"].ToString()
            });



        }
        [Authorize]
        [HttpPost]
        public IActionResult VerifyPhoneNumber(VerifyPhoneNumberDto numberDto)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var resultVerfiy = _userManager.VerifyChangePhoneNumberTokenAsync(user, numberDto.code, numberDto.PhoneNumber).Result;
            if (!resultVerfiy)
                ViewData["Message"] = $"Your code({numberDto.PhoneNumber}) is wrong ";
            else
            {
                user.PhoneNumberConfirmed = true;
                _userManager.UpdateAsync(user);
            }
            return View("VerifySucccess");
        }
        public IActionResult VerifySuccess()
        {
            return View();
        }
    }
}





