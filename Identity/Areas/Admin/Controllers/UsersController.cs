using Identity.Areas.Admin.Models.Dto;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<Users> _userManager;
        public UsersController(UserManager<Users> userManager)
        {
            _userManager = userManager; 
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Select(p => new UserListDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName = p.UserName,
                PhoneNumber = p.PhoneNumber,
                EmailConfirmed = p.EmailConfirmed,
                AccessFailedCount = p.AccessFailedCount

            }).ToList();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RegisterDto registerDto)
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
                return RedirectToAction("Index", "Users" , new { area="admin"});

            string message = "";
            foreach (var item in Users.Errors.ToList())
                message += item.Description + Environment.NewLine;

            TempData["Massege"] = message;
            return View(registerDto);
        }
    }
    
}
